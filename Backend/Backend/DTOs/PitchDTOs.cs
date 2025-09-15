using Backend.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class CreatePitchDto
{
    [Required]
    public string TeamId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public PitchCategory Category { get; set; }

    public string? ProblemStatement { get; set; }
    public string? Solution { get; set; }
    public string? TargetMarket { get; set; }
    public string? BusinessModel { get; set; }
    public string? CompetitiveAdvantage { get; set; }
    public decimal? FundingRequired { get; set; }
    public string? DemoUrl { get; set; }
    public string? PitchDeckUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
}

public class UpdatePitchDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PitchCategory? Category { get; set; }
    public string? ProblemStatement { get; set; }
    public string? Solution { get; set; }
    public string? TargetMarket { get; set; }
    public string? BusinessModel { get; set; }
    public string? CompetitiveAdvantage { get; set; }
    public decimal? FundingRequired { get; set; }
    public string? DemoUrl { get; set; }
    public string? PitchDeckUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
}

public class PitchDto
{
    public string Id { get; set; } = string.Empty;
    public string TeamId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PitchCategory Category { get; set; }
    public string? ProblemStatement { get; set; }
    public string? Solution { get; set; }
    public string? TargetMarket { get; set; }
    public string? BusinessModel { get; set; }
    public string? CompetitiveAdvantage { get; set; }
    public decimal? FundingRequired { get; set; }
    public string? DemoUrl { get; set; }
    public string? PitchDeckUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public TeamDto Team { get; set; } = new();
    public double AverageRating { get; set; }
    public int TotalVotes { get; set; }
    public double WeightedScore { get; set; }
}

public class PitchSummaryDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PitchCategory Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public TeamDto Team { get; set; } = new();
    public double AverageRating { get; set; }
    public int TotalVotes { get; set; }
    public double WeightedScore { get; set; }
}

public class LeaderboardDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public string? TeamLogoUrl { get; set; }
    public PitchCategory Category { get; set; }
    public double AverageRating { get; set; }
    public int TotalVotes { get; set; }
    public double WeightedScore { get; set; }
    public int Rank { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PitchFilterDto
{
    public PitchCategory? Category { get; set; }
    public string? SearchTitle { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 