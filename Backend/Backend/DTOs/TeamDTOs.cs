using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class CreateTeamDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public List<string> FounderIds { get; set; } = new();

    public string? LogoUrl { get; set; }

    public string? WebsiteUrl { get; set; }
}

public class UpdateTeamDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? FounderIds { get; set; }
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
}

public class TeamDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<UserDto> Founders { get; set; } = new();
    public string? LogoUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PitchCount { get; set; }
} 