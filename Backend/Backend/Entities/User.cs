using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Backend.Entities;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [BsonElement("password_hash")]
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [BsonElement("first_name")]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [BsonElement("last_name")]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [BsonElement("role")]
    [Required]
    public UserRole Role { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("is_active")]
    public bool IsActive { get; set; } = true;

    [BsonElement("profile_picture_url")]
    public string? ProfilePictureUrl { get; set; }
}

public enum UserRole
{
    Founder,
    Reviewer
} 