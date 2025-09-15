using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class CreateReviewDto
{
    [Required]
    public string PitchId { get; set; } = string.Empty;

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Feedback { get; set; }
}

public class ReviewDto
{
    public string Id { get; set; } = string.Empty;
    public string PitchId { get; set; } = string.Empty;
    public string ReviewerId { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string? Feedback { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PaginatedReviewsDto
{
    public List<ReviewDto> Reviews { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class ReviewStatsDto
{
    public double AverageRating { get; set; }
    public int TotalVotes { get; set; }
    public int[] RatingDistribution { get; set; } = new int[5]; // Index 0 = 1 star, Index 4 = 5 stars
} 