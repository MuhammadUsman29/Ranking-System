using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class Team
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string? Description { get; set; }

    [BsonElement("founder_ids")]
    public List<string> FounderIds { get; set; } = new();

    [BsonElement("logo_url")]
    public string? LogoUrl { get; set; }

    [BsonElement("website_url")]
    public string? WebsiteUrl { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    // Navigation property (not stored in MongoDB)
    [BsonIgnore]
    public List<User> Founders { get; set; } = new();

    // Navigation property (not stored in MongoDB)
    [BsonIgnore]
    public List<Pitch> Pitches { get; set; } = new();
} 