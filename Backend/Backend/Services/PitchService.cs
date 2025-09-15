using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class PitchService : IPitchService
{
    private readonly MongoDbContext _context;
    private readonly ITeamService _teamService;

    public PitchService(MongoDbContext context, ITeamService teamService)
    {
        _context = context;
        _teamService = teamService;
    }

    public async Task<PitchDto> CreatePitchAsync(CreatePitchDto createPitchDto, string userId)
    {
        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, createPitchDto.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can create pitches");
        }

        var pitch = new Pitch
        {
            TeamId = createPitchDto.TeamId,
            Title = createPitchDto.Title,
            Description = createPitchDto.Description,
            Category = createPitchDto.Category,
            ProblemStatement = createPitchDto.ProblemStatement,
            Solution = createPitchDto.Solution,
            TargetMarket = createPitchDto.TargetMarket,
            BusinessModel = createPitchDto.BusinessModel,
            CompetitiveAdvantage = createPitchDto.CompetitiveAdvantage,
            FundingRequired = createPitchDto.FundingRequired,
            DemoUrl = createPitchDto.DemoUrl,
            PitchDeckUrl = createPitchDto.PitchDeckUrl,
            DemoVideoUrl = createPitchDto.DemoVideoUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Pitches.InsertOneAsync(pitch);
        return await MapToPitchDtoAsync(pitch);
    }

    public async Task<PitchDto?> GetPitchByIdAsync(string pitchId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        return pitch != null ? await MapToPitchDtoAsync(pitch) : null;
    }

    public async Task<List<PitchSummaryDto>> GetPitchesByTeamAsync(string teamId)
    {
        var pitches = await _context.Pitches
            .Find(p => p.TeamId == teamId && p.IsActive)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();

        var pitchDtos = new List<PitchSummaryDto>();
        foreach (var pitch in pitches)
        {
            pitchDtos.Add(await MapToPitchSummaryDtoAsync(pitch));
        }

        return pitchDtos;
    }

    public async Task<List<PitchSummaryDto>> GetFilteredPitchesAsync(PitchFilterDto filter)
    {
        var filterBuilder = Builders<Pitch>.Filter;
        var filters = new List<FilterDefinition<Pitch>>
        {
            filterBuilder.Eq(p => p.IsActive, true)
        };

        // Category filter
        if (filter.Category.HasValue)
        {
            filters.Add(filterBuilder.Eq(p => p.Category, filter.Category.Value));
        }

        // Title search
        if (!string.IsNullOrWhiteSpace(filter.SearchTitle))
        {
            filters.Add(filterBuilder.Text(filter.SearchTitle));
        }

        var combinedFilter = filterBuilder.And(filters);

        var pitches = await _context.Pitches
            .Find(combinedFilter)
            .SortByDescending(p => p.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();

        var pitchDtos = new List<PitchSummaryDto>();
        foreach (var pitch in pitches)
        {
            pitchDtos.Add(await MapToPitchSummaryDtoAsync(pitch));
        }

        return pitchDtos;
    }

    public async Task<List<LeaderboardDto>> GetLeaderboardAsync(int limit = 50)
    {
        var pitches = await _context.Pitches
            .Find(p => p.IsActive)
            .ToListAsync();

        var leaderboardItems = new List<LeaderboardDto>();

        foreach (var pitch in pitches)
        {
            var stats = await GetPitchStatsAsync(pitch.Id);
            var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);

            if (team != null)
            {
                var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

                leaderboardItems.Add(new LeaderboardDto
                {
                    Id = pitch.Id,
                    Title = pitch.Title,
                    TeamName = team.Name,
                    TeamLogoUrl = team.LogoUrl,
                    Category = pitch.Category,
                    AverageRating = stats.AverageRating,
                    TotalVotes = stats.TotalVotes,
                    WeightedScore = weightedScore,
                    CreatedAt = pitch.CreatedAt
                });
            }
        }

        // Sort by weighted score descending, then by most recent
        var sortedLeaderboard = leaderboardItems
            .OrderByDescending(l => l.WeightedScore)
            .ThenByDescending(l => l.CreatedAt)
            .Take(limit)
            .ToList();

        // Assign ranks
        for (int i = 0; i < sortedLeaderboard.Count; i++)
        {
            sortedLeaderboard[i].Rank = i + 1;
        }

        return sortedLeaderboard;
    }

    public async Task<PitchDto> UpdatePitchAsync(string pitchId, UpdatePitchDto updatePitchDto, string userId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            throw new ArgumentException("Pitch not found");
        }

        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, pitch.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can update pitches");
        }

        // Update only provided fields
        var updateDefinition = Builders<Pitch>.Update.Set(p => p.UpdatedAt, DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(updatePitchDto.Title))
            updateDefinition = updateDefinition.Set(p => p.Title, updatePitchDto.Title);

        if (!string.IsNullOrWhiteSpace(updatePitchDto.Description))
            updateDefinition = updateDefinition.Set(p => p.Description, updatePitchDto.Description);

        if (updatePitchDto.Category.HasValue)
            updateDefinition = updateDefinition.Set(p => p.Category, updatePitchDto.Category.Value);

        if (updatePitchDto.ProblemStatement != null)
            updateDefinition = updateDefinition.Set(p => p.ProblemStatement, updatePitchDto.ProblemStatement);

        if (updatePitchDto.Solution != null)
            updateDefinition = updateDefinition.Set(p => p.Solution, updatePitchDto.Solution);

        if (updatePitchDto.TargetMarket != null)
            updateDefinition = updateDefinition.Set(p => p.TargetMarket, updatePitchDto.TargetMarket);

        if (updatePitchDto.BusinessModel != null)
            updateDefinition = updateDefinition.Set(p => p.BusinessModel, updatePitchDto.BusinessModel);

        if (updatePitchDto.CompetitiveAdvantage != null)
            updateDefinition = updateDefinition.Set(p => p.CompetitiveAdvantage, updatePitchDto.CompetitiveAdvantage);

        if (updatePitchDto.FundingRequired.HasValue)
            updateDefinition = updateDefinition.Set(p => p.FundingRequired, updatePitchDto.FundingRequired);

        if (updatePitchDto.DemoUrl != null)
            updateDefinition = updateDefinition.Set(p => p.DemoUrl, updatePitchDto.DemoUrl);

        if (updatePitchDto.PitchDeckUrl != null)
            updateDefinition = updateDefinition.Set(p => p.PitchDeckUrl, updatePitchDto.PitchDeckUrl);

        if (updatePitchDto.DemoVideoUrl != null)
            updateDefinition = updateDefinition.Set(p => p.DemoVideoUrl, updatePitchDto.DemoVideoUrl);

        await _context.Pitches.UpdateOneAsync(p => p.Id == pitchId, updateDefinition);

        var updatedPitch = await _context.Pitches
            .Find(p => p.Id == pitchId)
            .FirstOrDefaultAsync();

        return await MapToPitchDtoAsync(updatedPitch!);
    }

    public async Task<bool> DeletePitchAsync(string pitchId, string userId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            return false;
        }

        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, pitch.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can delete pitches");
        }

        // Soft delete - mark as inactive
        var updateDefinition = Builders<Pitch>.Update
            .Set(p => p.IsActive, false)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Pitches.UpdateOneAsync(p => p.Id == pitchId, updateDefinition);
        return result.ModifiedCount > 0;
    }

    public async Task<ReviewStatsDto> GetPitchStatsAsync(string pitchId)
    {
        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .ToListAsync();

        if (!reviews.Any())
        {
            return new ReviewStatsDto
            {
                AverageRating = 0,
                TotalVotes = 0,
                RatingDistribution = new int[5]
            };
        }

        var averageRating = reviews.Average(r => r.Rating);
        var totalVotes = reviews.Count;
        var ratingDistribution = new int[5];

        foreach (var review in reviews)
        {
            ratingDistribution[review.Rating - 1]++; // Rating 1 goes to index 0, etc.
        }

        return new ReviewStatsDto
        {
            AverageRating = Math.Round(averageRating, 2),
            TotalVotes = totalVotes,
            RatingDistribution = ratingDistribution
        };
    }

    public double CalculateWeightedScore(double averageRating, int totalVotes)
    {
        if (totalVotes == 0) return 0;
        
        // Formula: weighted_score = (avg_rating * log10(total_votes + 1))
        return Math.Round(averageRating * Math.Log10(totalVotes + 1), 2);
    }

    private async Task<PitchDto> MapToPitchDtoAsync(Pitch pitch)
    {
        var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);
        var stats = await GetPitchStatsAsync(pitch.Id);
        var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

        return new PitchDto
        {
            Id = pitch.Id,
            TeamId = pitch.TeamId,
            Title = pitch.Title,
            Description = pitch.Description,
            Category = pitch.Category,
            ProblemStatement = pitch.ProblemStatement,
            Solution = pitch.Solution,
            TargetMarket = pitch.TargetMarket,
            BusinessModel = pitch.BusinessModel,
            CompetitiveAdvantage = pitch.CompetitiveAdvantage,
            FundingRequired = pitch.FundingRequired,
            DemoUrl = pitch.DemoUrl,
            PitchDeckUrl = pitch.PitchDeckUrl,
            DemoVideoUrl = pitch.DemoVideoUrl,
            CreatedAt = pitch.CreatedAt,
            Team = team ?? new TeamDto(),
            AverageRating = stats.AverageRating,
            TotalVotes = stats.TotalVotes,
            WeightedScore = weightedScore
        };
    }

    private async Task<PitchSummaryDto> MapToPitchSummaryDtoAsync(Pitch pitch)
    {
        var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);
        var stats = await GetPitchStatsAsync(pitch.Id);
        var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

        return new PitchSummaryDto
        {
            Id = pitch.Id,
            Title = pitch.Title,
            Description = pitch.Description,
            Category = pitch.Category,
            CreatedAt = pitch.CreatedAt,
            Team = team ?? new TeamDto(),
            AverageRating = stats.AverageRating,
            TotalVotes = stats.TotalVotes,
            WeightedScore = weightedScore
        };
    }
} 