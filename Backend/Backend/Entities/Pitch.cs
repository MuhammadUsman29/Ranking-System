using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class Pitch
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("team_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string TeamId { get; set; } = string.Empty;

    [BsonElement("title")]
    [Required]
    public string Title { get; set; } = string.Empty;

    [BsonElement("description")]
    [Required]
    public string Description { get; set; } = string.Empty;

    [BsonElement("category")]
    [Required]
    public PitchCategory Category { get; set; }

    [BsonElement("problem_statement")]
    public string? ProblemStatement { get; set; }

    [BsonElement("solution")]
    public string? Solution { get; set; }

    [BsonElement("target_market")]
    public string? TargetMarket { get; set; }

    [BsonElement("business_model")]
    public string? BusinessModel { get; set; }

    [BsonElement("competitive_advantage")]
    public string? CompetitiveAdvantage { get; set; }

    [BsonElement("funding_required")]
    public decimal? FundingRequired { get; set; }

    [BsonElement("demo_url")]
    public string? DemoUrl { get; set; }

    [BsonElement("pitch_deck_url")]
    public string? PitchDeckUrl { get; set; }

    [BsonElement("demo_video_url")]
    public string? DemoVideoUrl { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    [BsonElement("submission_deadline")]
    public DateTime? SubmissionDeadline { get; set; }

    // Calculated fields (not stored in MongoDB)
    [BsonIgnore]
    public double AverageRating { get; set; }

    [BsonIgnore]
    public int TotalVotes { get; set; }

    [BsonIgnore]
    public double WeightedScore { get; set; }

    // Navigation properties (not stored in MongoDB)
    [BsonIgnore]
    public Team Team { get; set; } = new();

    [BsonIgnore]
    public List<Review> Reviews { get; set; } = new();
}

public enum PitchCategory
{
    FinTech,
    HealthTech,
    AI,
    Other
} 