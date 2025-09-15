using Backend.DTOs;

namespace Backend.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<bool> ValidateTokenAsync(string token);
    string GenerateJwtToken(UserDto user);
} 