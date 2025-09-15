using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("pitch_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string PitchId { get; set; } = string.Empty;

    [BsonElement("reviewer_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    [Required]
    public string ReviewerId { get; set; } = string.Empty;

    [BsonElement("rating")]
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [BsonElement("feedback")]
    public string? Feedback { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties (not stored in MongoDB)
    [BsonIgnore]
    public User Reviewer { get; set; } = new();

    [BsonIgnore]
    public Pitch Pitch { get; set; } = new();
} 