using Backend.DTOs;

namespace Backend.Services;

public interface ITeamService
{
    Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, string creatorId);
    Task<TeamDto?> GetTeamByIdAsync(string teamId);
    Task<List<TeamDto>> GetTeamsByFounderAsync(string founderId);
    Task<TeamDto> UpdateTeamAsync(string teamId, UpdateTeamDto updateTeamDto, string userId);
    Task<bool> DeleteTeamAsync(string teamId, string userId);
    Task<List<TeamDto>> GetAllTeamsAsync();
    Task<bool> IsUserFounderOfTeamAsync(string userId, string teamId);
} 