using Backend.DTOs;

namespace Backend.Services;

public interface IPitchService
{
    Task<PitchDto> CreatePitchAsync(CreatePitchDto createPitchDto, string userId);
    Task<PitchDto?> GetPitchByIdAsync(string pitchId);
    Task<List<PitchSummaryDto>> GetPitchesByTeamAsync(string teamId);
    Task<List<PitchSummaryDto>> GetFilteredPitchesAsync(PitchFilterDto filter);
    Task<List<LeaderboardDto>> GetLeaderboardAsync(int limit = 50);
    Task<PitchDto> UpdatePitchAsync(string pitchId, UpdatePitchDto updatePitchDto, string userId);
    Task<bool> DeletePitchAsync(string pitchId, string userId);
    Task<ReviewStatsDto> GetPitchStatsAsync(string pitchId);
    double CalculateWeightedScore(double averageRating, int totalVotes);
} 