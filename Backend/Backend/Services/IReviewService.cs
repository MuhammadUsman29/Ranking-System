using Backend.DTOs;

namespace Backend.Services;

public interface IReviewService
{
    Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto, string reviewerId);
    Task<PaginatedReviewsDto> GetPitchReviewsAsync(string pitchId, int page = 1, int pageSize = 5);
    Task<List<ReviewDto>> GetLatestReviewsForPitchAsync(string pitchId, int count = 5);
    Task<ReviewDto?> GetUserReviewForPitchAsync(string userId, string pitchId);
    Task<bool> HasUserReviewedPitchAsync(string userId, string pitchId);
    Task<List<ReviewDto>> GetReviewsByUserAsync(string userId);
} 