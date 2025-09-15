using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class TeamService : ITeamService
{
    private readonly MongoDbContext _context;
    private readonly IAuthService _authService;

    public TeamService(MongoDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, string creatorId)
    {
        // Validate that all founder IDs exist and are founders
        var founders = await _context.Users
            .Find(u => createTeamDto.FounderIds.Contains(u.Id) && u.Role == UserRole.Founder && u.IsActive)
            .ToListAsync();

        if (founders.Count != createTeamDto.FounderIds.Count)
        {
            throw new ArgumentException("One or more founder IDs are invalid or not founders");
        }

        // Ensure creator is included in founders list
        if (!createTeamDto.FounderIds.Contains(creatorId))
        {
            createTeamDto.FounderIds.Add(creatorId);
        }

        var team = new Team
        {
            Name = createTeamDto.Name,
            Description = createTeamDto.Description,
            FounderIds = createTeamDto.FounderIds,
            LogoUrl = createTeamDto.LogoUrl,
            WebsiteUrl = createTeamDto.WebsiteUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Teams.InsertOneAsync(team);

        return await MapToTeamDtoAsync(team);
    }

    public async Task<TeamDto?> GetTeamByIdAsync(string teamId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        return team != null ? await MapToTeamDtoAsync(team) : null;
    }

    public async Task<List<TeamDto>> GetTeamsByFounderAsync(string founderId)
    {
        var teams = await _context.Teams
            .Find(t => t.FounderIds.Contains(founderId) && t.IsActive)
            .ToListAsync();

        var teamDtos = new List<TeamDto>();
        foreach (var team in teams)
        {
            teamDtos.Add(await MapToTeamDtoAsync(team));
        }

        return teamDtos;
    }

    public async Task<TeamDto> UpdateTeamAsync(string teamId, UpdateTeamDto updateTeamDto, string userId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team == null)
        {
            throw new ArgumentException("Team not found");
        }

        // Check if user is a founder of this team
        if (!team.FounderIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("Only team founders can update the team");
        }

        // Update only provided fields
        var updateDefinition = Builders<Team>.Update.Set(t => t.UpdatedAt, DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(updateTeamDto.Name))
            updateDefinition = updateDefinition.Set(t => t.Name, updateTeamDto.Name);

        if (updateTeamDto.Description != null)
            updateDefinition = updateDefinition.Set(t => t.Description, updateTeamDto.Description);

        if (updateTeamDto.FounderIds != null)
        {
            // Validate new founder IDs
            var founders = await _context.Users
                .Find(u => updateTeamDto.FounderIds.Contains(u.Id) && u.Role == UserRole.Founder && u.IsActive)
                .ToListAsync();

            if (founders.Count != updateTeamDto.FounderIds.Count)
            {
                throw new ArgumentException("One or more founder IDs are invalid or not founders");
            }

            updateDefinition = updateDefinition.Set(t => t.FounderIds, updateTeamDto.FounderIds);
        }

        if (updateTeamDto.LogoUrl != null)
            updateDefinition = updateDefinition.Set(t => t.LogoUrl, updateTeamDto.LogoUrl);

        if (updateTeamDto.WebsiteUrl != null)
            updateDefinition = updateDefinition.Set(t => t.WebsiteUrl, updateTeamDto.WebsiteUrl);

        await _context.Teams.UpdateOneAsync(t => t.Id == teamId, updateDefinition);

        var updatedTeam = await _context.Teams
            .Find(t => t.Id == teamId)
            .FirstOrDefaultAsync();

        return await MapToTeamDtoAsync(updatedTeam!);
    }

    public async Task<bool> DeleteTeamAsync(string teamId, string userId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team == null)
        {
            return false;
        }

        // Check if user is a founder of this team
        if (!team.FounderIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("Only team founders can delete the team");
        }

        // Soft delete - mark as inactive
        var updateDefinition = Builders<Team>.Update
            .Set(t => t.IsActive, false)
            .Set(t => t.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Teams.UpdateOneAsync(t => t.Id == teamId, updateDefinition);
        return result.ModifiedCount > 0;
    }

    public async Task<List<TeamDto>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams
            .Find(t => t.IsActive)
            .SortByDescending(t => t.CreatedAt)
            .ToListAsync();

        var teamDtos = new List<TeamDto>();
        foreach (var team in teams)
        {
            teamDtos.Add(await MapToTeamDtoAsync(team));
        }

        return teamDtos;
    }

    public async Task<bool> IsUserFounderOfTeamAsync(string userId, string teamId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        return team?.FounderIds.Contains(userId) ?? false;
    }

    private async Task<TeamDto> MapToTeamDtoAsync(Team team)
    {
        // Get founder details
        var founders = await _context.Users
            .Find(u => team.FounderIds.Contains(u.Id) && u.IsActive)
            .ToListAsync();

        // Get pitch count
        var pitchCount = (int)await _context.Pitches
            .CountDocumentsAsync(p => p.TeamId == team.Id && p.IsActive);

        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            Founders = founders.Select(f => new UserDto
            {
                Id = f.Id,
                Email = f.Email,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Role = f.Role,
                ProfilePictureUrl = f.ProfilePictureUrl,
                CreatedAt = f.CreatedAt
            }).ToList(),
            LogoUrl = team.LogoUrl,
            WebsiteUrl = team.WebsiteUrl,
            CreatedAt = team.CreatedAt,
            PitchCount = pitchCount
        };
    }
} 