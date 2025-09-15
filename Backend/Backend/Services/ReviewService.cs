using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class ReviewService : IReviewService
{
    private readonly MongoDbContext _context;
    private readonly IAuthService _authService;

    public ReviewService(MongoDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto, string reviewerId)
    {
        // Verify reviewer exists and is a reviewer
        var reviewer = await _authService.GetUserByIdAsync(reviewerId);
        if (reviewer == null || reviewer.Role != UserRole.Reviewer)
        {
            throw new UnauthorizedAccessException("Only reviewers can create reviews");
        }

        // Verify pitch exists
        var pitch = await _context.Pitches
            .Find(p => p.Id == createReviewDto.PitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            throw new ArgumentException("Pitch not found");
        }

        // Check if user has already reviewed this pitch
        var existingReview = await _context.Reviews
            .Find(r => r.PitchId == createReviewDto.PitchId && r.ReviewerId == reviewerId)
            .FirstOrDefaultAsync();

        if (existingReview != null)
        {
            throw new InvalidOperationException("You have already reviewed this pitch");
        }

        // Prevent reviewers from reviewing their own team's pitches
        var team = await _context.Teams
            .Find(t => t.Id == pitch.TeamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team != null && team.FounderIds.Contains(reviewerId))
        {
            throw new UnauthorizedAccessException("You cannot review your own team's pitch");
        }

        var review = new Review
        {
            PitchId = createReviewDto.PitchId,
            ReviewerId = reviewerId,
            Rating = createReviewDto.Rating,
            Feedback = createReviewDto.Feedback,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Reviews.InsertOneAsync(review);
        return await MapToReviewDtoAsync(review);
    }

    public async Task<PaginatedReviewsDto> GetPitchReviewsAsync(string pitchId, int page = 1, int pageSize = 5)
    {
        var totalCount = (int)await _context.Reviews
            .CountDocumentsAsync(r => r.PitchId == pitchId);

        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return new PaginatedReviewsDto
        {
            Reviews = reviewDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }

    public async Task<List<ReviewDto>> GetLatestReviewsForPitchAsync(string pitchId, int count = 5)
    {
        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .SortByDescending(r => r.CreatedAt)
            .Limit(count)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return reviewDtos;
    }

    public async Task<ReviewDto?> GetUserReviewForPitchAsync(string userId, string pitchId)
    {
        var review = await _context.Reviews
            .Find(r => r.ReviewerId == userId && r.PitchId == pitchId)
            .FirstOrDefaultAsync();

        return review != null ? await MapToReviewDtoAsync(review) : null;
    }

    public async Task<bool> HasUserReviewedPitchAsync(string userId, string pitchId)
    {
        var count = await _context.Reviews
            .CountDocumentsAsync(r => r.ReviewerId == userId && r.PitchId == pitchId);

        return count > 0;
    }

    public async Task<List<ReviewDto>> GetReviewsByUserAsync(string userId)
    {
        var reviews = await _context.Reviews
            .Find(r => r.ReviewerId == userId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return reviewDtos;
    }

    private async Task<ReviewDto> MapToReviewDtoAsync(Review review)
    {
        var reviewer = await _authService.GetUserByIdAsync(review.ReviewerId);

        return new ReviewDto
        {
            Id = review.Id,
            PitchId = review.PitchId,
            ReviewerId = review.ReviewerId,
            ReviewerName = reviewer != null ? $"{reviewer.FirstName} {reviewer.LastName}" : "Unknown Reviewer",
            Rating = review.Rating,
            Feedback = review.Feedback,
            CreatedAt = review.CreatedAt
        };
    }
} 