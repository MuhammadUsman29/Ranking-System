# Build a startup pitch showcase system
_Exported on 9/15/2025 at 12:38:03 GMT+5 from Cursor (1.2.2)_

---

**User**

Prompt: Your task is to Act as a Full Stack .Net Core and Angular developer and build a "Startup Pitch Showcase — Ranking System" according to the following Requirements (keep asking questions and feedback so we can build iteratively):

Requirements:
  1)Create a signup and login Functionality.
  2)we will have these roles who can signup or login:
         #Founders can create a team and submit a Pitch.
          #Reviewers browse pitches, filter by category, search by tilte and can  rate on a scale of 1-5 and can also give a shor feedback in text form.

3)Main Screens:
#Leaderboard Home page : shows Teams rankled by Weighted Score:
Formula: weighted_score = (avg_rating * log10(total_votes + 1))
 Sort temas by most recent.
#Pitch page:
-Pitch details such as average rating, total votes,
- latest 5 feedback items and also apply pagination.

Mock Data:
Seed Data: at least 80 mock teams and 120 mock pitches you can keep some ptiches empty.

Technologies to use:
Frotnend: Angular 18
Backend : .Net Core
Database: MongoDB
Architecture: Use serverless Architecture.






      




 

---

**Cursor**

I'll help you build this "Startup Pitch Showcase — Ranking System" step by step. This is an exciting project! Let me break down what we need to build and start by asking some clarifying questions to ensure we build exactly what you need.

## Clarifying Questions:

Before I start building, I need to understand a few details better:

1. **Serverless Architecture**: When you mention serverless, are you thinking:
   - Azure Functions for the backend APIs?
   - AWS Lambda?
   - Or do you mean containerized deployment (like Azure Container Apps)?

2. **Pitch Categories**: What categories should be available for pitches? (e.g., FinTech, HealthTech, EdTech, E-commerce, etc.)

3. **Team Structure**: 
   - Can a founder be part of multiple teams?
   - Should teams have multiple founders or just one founder per team?
   - Any limit on team size?

4. **Pitch Submission**:
   - What fields should a pitch have? (Title, Description, Category, Demo URL, etc.)
   - Any file upload requirements (pitch deck, demo video)?
   - Should there be a submission deadline or is it always open?

5. **Rating System**:
   - Should reviewers be able to update their ratings?
   - Any restrictions on who can review what (e.g., can't review own pitch)?

6. **UI/UX Preferences**:
   - Any specific design framework preference? (Angular Material, Bootstrap, PrimeNG?)
   - Color scheme or branding requirements?

Let me start by setting up the basic project structure while you think about these questions. I'll begin with the backend .NET Core setup:

---

**User**

Answers to your questions:
1: AWs Lambda
2:Fintech/Health/AI/Other
3:Team can have multiple founders no limit.
4:yes include all.
5:No rating cannot be updated
6:use primeNG
NOte: please build frontned and backend in seprate folders.

---

**Cursor**

Perfect! Thank you for the clarifications. I'll build the frontend and backend in separate folders with AWS Lambda for serverless architecture and PrimeNG for the UI. Let me start setting up the project structure.

---

**User**

don't setup projects.just use seprate folders for backend and frontend with proper architecture and code files.

---

**Cursor**

Got it! I'll create the proper folder structure and code files directly without setting up projects. Let me build the complete architecture with separate backend and frontend folders.

---

**User**

I have craeted a web api proejct with the name of Backend in C:\Users\usman.u\Desktop\AI based Task\Ranking-System
pleae use that for .net core4 code and code in that then we will move to frotnend.

---

**Cursor**

Perfect! I'll work with the existing Backend Web API project in the Ranking-System folder. Let me explore the current structure and start building the .NET Core backend code there.

Great! I can see the Web API project is already set up. Let me examine the current files and then start building our Pitch Showcase system.

Now I'll start building the backend architecture. Let me update the project with the necessary dependencies and create the proper folder structure.

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
    <PackageReference Include="MongoDB.Driver" Version="2.25.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.0.3" />
    <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
    <PackageReference Include="Amazon.Lambda.AspNetCoreServer" Version="8.1.0" />
    <PackageReference Include="Amazon.Lambda.Core" Version="2.2.0" />
    <PackageReference Include="Amazon.Lambda.Serialization.SystemTextJson" Version="2.4.1" />
    <PackageReference Include="AWSSDK.Extensions.NETCore.Setup" Version="3.7.300" />
  </ItemGroup>

</Project>
```

Now let me create the proper folder structure for our backend architecture:

Let me create the directories using individual commands:

---

**User**

Build models in Entities folder.

---

**Cursor**

Perfect! I'll create the Models in an Entities folder. Let me create the folder structure and build all the necessary data models for our Pitch Showcase system.

Now let me create all the entity models for our Pitch Showcase system:

```csharp
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
```

```csharp
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
```

```csharp
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
```

```csharp
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
```

Now let me create the DTOs (Data Transfer Objects) for API communication:

```csharp
using Backend.Entities;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
    public DateTime ExpiresAt { get; set; }
}

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

```csharp
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
```

```csharp
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
```

```csharp
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
```

Now let me create the MongoDB configuration and database context:

```csharp
namespace Backend.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string UsersCollectionName { get; set; } = "users";
    public string TeamsCollectionName { get; set; } = "teams";
    public string PitchesCollectionName { get; set; } = "pitches";
    public string ReviewsCollectionName { get; set; } = "reviews";
}
```

```csharp
namespace Backend.Configuration;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationInMinutes { get; set; } = 60;
}
```

```csharp
using Backend.Configuration;
using Backend.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<User> Users => 
        _database.GetCollection<User>(_settings.UsersCollectionName);

    public IMongoCollection<Team> Teams => 
        _database.GetCollection<Team>(_settings.TeamsCollectionName);

    public IMongoCollection<Pitch> Pitches => 
        _database.GetCollection<Pitch>(_settings.PitchesCollectionName);

    public IMongoCollection<Review> Reviews => 
        _database.GetCollection<Review>(_settings.ReviewsCollectionName);

    public async Task CreateIndexesAsync()
    {
        // User indexes
        await Users.Indexes.CreateOneAsync(
            new CreateIndexModel<User>(Builders<User>.IndexKeys.Ascending(u => u.Email)));

        // Team indexes
        await Teams.Indexes.CreateOneAsync(
            new CreateIndexModel<Team>(Builders<Team>.IndexKeys.Ascending(t => t.Name)));

        // Pitch indexes
        await Pitches.Indexes.CreateOneAsync(
            new CreateIndexModel<Pitch>(Builders<Pitch>.IndexKeys.Ascending(p => p.TeamId)));
        await Pitches.Indexes.CreateOneAsync(
            new CreateIndexModel<Pitch>(Builders<Pitch>.IndexKeys.Ascending(p => p.Category)));
        await Pitches.Indexes.CreateOneAsync(
            new CreateIndexModel<Pitch>(Builders<Pitch>.IndexKeys.Text(p => p.Title)));

        // Review indexes
        await Reviews.Indexes.CreateOneAsync(
            new CreateIndexModel<Review>(Builders<Review>.IndexKeys.Ascending(r => r.PitchId)));
        await Reviews.Indexes.CreateOneAsync(
            new CreateIndexModel<Review>(Builders<Review>.IndexKeys.Ascending(r => r.ReviewerId)));
        
        // Compound index for preventing duplicate reviews
        await Reviews.Indexes.CreateOneAsync(
            new CreateIndexModel<Review>(
                Builders<Review>.IndexKeys.Ascending(r => r.PitchId).Ascending(r => r.ReviewerId),
                new CreateIndexOptions { Unique = true }));
    }
}
```

Perfect! I've created all the entity models and DTOs. Now let me show you what we have built so far:

## ✅ Completed Backend Architecture:

### **Entities (Models):**
- **User**: Email, password, role (Founder/Reviewer), profile info
- **Team**: Name, description, multiple founders, logo, website
- **Pitch**: Complete pitch details with category, funding, demo URLs, calculated ratings
- **Review**: Rating (1-5), feedback, reviewer info (no updates allowed)

### **DTOs (Data Transfer Objects):**
- **Auth DTOs**: Login, Register, JWT response, User profile
- **Team DTOs**: Create, Update, Display with founder info
- **Pitch DTOs**: Create, Update, Display, Summary, Leaderboard, Filtering
- **Review DTOs**: Create, Display, Pagination, Stats

### **Configuration & Data Layer:**
- **MongoDB Settings**: Connection and collection configuration
- **JWT Settings**: Authentication configuration  
- **MongoDB Context**: Database operations with proper indexing

### **Key Features Implemented:**
✅ Role-based system (Founders vs Reviewers)  
✅ Teams with multiple founders  
✅ Comprehensive pitch data model  
✅ Rating system (1-5) with no updates  
✅ Weighted scoring formula support  
✅ Category filtering (FinTech/HealthTech/AI/Other)  
✅ Search by title capability  
✅ Pagination support  
✅ MongoDB indexing for performance  

## **Next Steps:**

Would you like me to continue with:

1. **Services Layer** - Business logic for authentication, teams, pitches, reviews
2. **Controllers** - API endpoints for all operations  
3. **JWT Authentication & Middleware** - Security implementation
4. **Mock Data Generation** - 80+ teams and 120+ pitches
5. **AWS Lambda Configuration** - Serverless deployment setup

Which would you prefer to tackle next? I recommend starting with the Services layer to build the business logic, then moving to Controllers for the API endpoints.

---

**User**

yes

---

**Cursor**

Great! Let me continue with the Services layer to build the business logic. I'll start by creating the authentication service and then move to the other services.

```csharp
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
```

```csharp
using Backend.Configuration;
using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly MongoDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(MongoDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // Check if user already exists
        var existingUser = await _context.Users
            .Find(u => u.Email.ToLower() == request.Email.ToLower())
            .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        // Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create new user
        var user = new User
        {
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Users.InsertOneAsync(user);

        var userDto = MapToUserDto(user);
        var token = GenerateJwtToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _context.Users
            .Find(u => u.Email.ToLower() == request.Email.ToLower() && u.IsActive)
            .FirstOrDefaultAsync();

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var userDto = MapToUserDto(user);
        var token = GenerateJwtToken(userDto);

        return new AuthResponseDto
        {
            Token = token,
            User = userDto,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        };
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _context.Users
            .Find(u => u.Id == userId && u.IsActive)
            .FirstOrDefaultAsync();

        return user != null ? MapToUserDto(user) : null;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public string GenerateJwtToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            ProfilePictureUrl = user.ProfilePictureUrl,
            CreatedAt = user.CreatedAt
        };
    }
}
```

```csharp
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
```

```csharp
using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class TeamService : ITeamService
{
    private readonly MongoDbContext _context;
    private readonly IAuthService _authService;

    public TeamService(MongoDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto, string creatorId)
    {
        // Validate that all founder IDs exist and are founders
        var founders = await _context.Users
            .Find(u => createTeamDto.FounderIds.Contains(u.Id) && u.Role == UserRole.Founder && u.IsActive)
            .ToListAsync();

        if (founders.Count != createTeamDto.FounderIds.Count)
        {
            throw new ArgumentException("One or more founder IDs are invalid or not founders");
        }

        // Ensure creator is included in founders list
        if (!createTeamDto.FounderIds.Contains(creatorId))
        {
            createTeamDto.FounderIds.Add(creatorId);
        }

        var team = new Team
        {
            Name = createTeamDto.Name,
            Description = createTeamDto.Description,
            FounderIds = createTeamDto.FounderIds,
            LogoUrl = createTeamDto.LogoUrl,
            WebsiteUrl = createTeamDto.WebsiteUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Teams.InsertOneAsync(team);

        return await MapToTeamDtoAsync(team);
    }

    public async Task<TeamDto?> GetTeamByIdAsync(string teamId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        return team != null ? await MapToTeamDtoAsync(team) : null;
    }

    public async Task<List<TeamDto>> GetTeamsByFounderAsync(string founderId)
    {
        var teams = await _context.Teams
            .Find(t => t.FounderIds.Contains(founderId) && t.IsActive)
            .ToListAsync();

        var teamDtos = new List<TeamDto>();
        foreach (var team in teams)
        {
            teamDtos.Add(await MapToTeamDtoAsync(team));
        }

        return teamDtos;
    }

    public async Task<TeamDto> UpdateTeamAsync(string teamId, UpdateTeamDto updateTeamDto, string userId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team == null)
        {
            throw new ArgumentException("Team not found");
        }

        // Check if user is a founder of this team
        if (!team.FounderIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("Only team founders can update the team");
        }

        // Update only provided fields
        var updateDefinition = Builders<Team>.Update.Set(t => t.UpdatedAt, DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(updateTeamDto.Name))
            updateDefinition = updateDefinition.Set(t => t.Name, updateTeamDto.Name);

        if (updateTeamDto.Description != null)
            updateDefinition = updateDefinition.Set(t => t.Description, updateTeamDto.Description);

        if (updateTeamDto.FounderIds != null)
        {
            // Validate new founder IDs
            var founders = await _context.Users
                .Find(u => updateTeamDto.FounderIds.Contains(u.Id) && u.Role == UserRole.Founder && u.IsActive)
                .ToListAsync();

            if (founders.Count != updateTeamDto.FounderIds.Count)
            {
                throw new ArgumentException("One or more founder IDs are invalid or not founders");
            }

            updateDefinition = updateDefinition.Set(t => t.FounderIds, updateTeamDto.FounderIds);
        }

        if (updateTeamDto.LogoUrl != null)
            updateDefinition = updateDefinition.Set(t => t.LogoUrl, updateTeamDto.LogoUrl);

        if (updateTeamDto.WebsiteUrl != null)
            updateDefinition = updateDefinition.Set(t => t.WebsiteUrl, updateTeamDto.WebsiteUrl);

        await _context.Teams.UpdateOneAsync(t => t.Id == teamId, updateDefinition);

        var updatedTeam = await _context.Teams
            .Find(t => t.Id == teamId)
            .FirstOrDefaultAsync();

        return await MapToTeamDtoAsync(updatedTeam!);
    }

    public async Task<bool> DeleteTeamAsync(string teamId, string userId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team == null)
        {
            return false;
        }

        // Check if user is a founder of this team
        if (!team.FounderIds.Contains(userId))
        {
            throw new UnauthorizedAccessException("Only team founders can delete the team");
        }

        // Soft delete - mark as inactive
        var updateDefinition = Builders<Team>.Update
            .Set(t => t.IsActive, false)
            .Set(t => t.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Teams.UpdateOneAsync(t => t.Id == teamId, updateDefinition);
        return result.ModifiedCount > 0;
    }

    public async Task<List<TeamDto>> GetAllTeamsAsync()
    {
        var teams = await _context.Teams
            .Find(t => t.IsActive)
            .SortByDescending(t => t.CreatedAt)
            .ToListAsync();

        var teamDtos = new List<TeamDto>();
        foreach (var team in teams)
        {
            teamDtos.Add(await MapToTeamDtoAsync(team));
        }

        return teamDtos;
    }

    public async Task<bool> IsUserFounderOfTeamAsync(string userId, string teamId)
    {
        var team = await _context.Teams
            .Find(t => t.Id == teamId && t.IsActive)
            .FirstOrDefaultAsync();

        return team?.FounderIds.Contains(userId) ?? false;
    }

    private async Task<TeamDto> MapToTeamDtoAsync(Team team)
    {
        // Get founder details
        var founders = await _context.Users
            .Find(u => team.FounderIds.Contains(u.Id) && u.IsActive)
            .ToListAsync();

        // Get pitch count
        var pitchCount = (int)await _context.Pitches
            .CountDocumentsAsync(p => p.TeamId == team.Id && p.IsActive);

        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            Description = team.Description,
            Founders = founders.Select(f => new UserDto
            {
                Id = f.Id,
                Email = f.Email,
                FirstName = f.FirstName,
                LastName = f.LastName,
                Role = f.Role,
                ProfilePictureUrl = f.ProfilePictureUrl,
                CreatedAt = f.CreatedAt
            }).ToList(),
            LogoUrl = team.LogoUrl,
            WebsiteUrl = team.WebsiteUrl,
            CreatedAt = team.CreatedAt,
            PitchCount = pitchCount
        };
    }
}
```

Now let me create the Pitch service with the weighted scoring algorithm:

```csharp
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
```

```csharp
using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class PitchService : IPitchService
{
    private readonly MongoDbContext _context;
    private readonly ITeamService _teamService;

    public PitchService(MongoDbContext context, ITeamService teamService)
    {
        _context = context;
        _teamService = teamService;
    }

    public async Task<PitchDto> CreatePitchAsync(CreatePitchDto createPitchDto, string userId)
    {
        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, createPitchDto.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can create pitches");
        }

        var pitch = new Pitch
        {
            TeamId = createPitchDto.TeamId,
            Title = createPitchDto.Title,
            Description = createPitchDto.Description,
            Category = createPitchDto.Category,
            ProblemStatement = createPitchDto.ProblemStatement,
            Solution = createPitchDto.Solution,
            TargetMarket = createPitchDto.TargetMarket,
            BusinessModel = createPitchDto.BusinessModel,
            CompetitiveAdvantage = createPitchDto.CompetitiveAdvantage,
            FundingRequired = createPitchDto.FundingRequired,
            DemoUrl = createPitchDto.DemoUrl,
            PitchDeckUrl = createPitchDto.PitchDeckUrl,
            DemoVideoUrl = createPitchDto.DemoVideoUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Pitches.InsertOneAsync(pitch);
        return await MapToPitchDtoAsync(pitch);
    }

    public async Task<PitchDto?> GetPitchByIdAsync(string pitchId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        return pitch != null ? await MapToPitchDtoAsync(pitch) : null;
    }

    public async Task<List<PitchSummaryDto>> GetPitchesByTeamAsync(string teamId)
    {
        var pitches = await _context.Pitches
            .Find(p => p.TeamId == teamId && p.IsActive)
            .SortByDescending(p => p.CreatedAt)
            .ToListAsync();

        var pitchDtos = new List<PitchSummaryDto>();
        foreach (var pitch in pitches)
        {
            pitchDtos.Add(await MapToPitchSummaryDtoAsync(pitch));
        }

        return pitchDtos;
    }

    public async Task<List<PitchSummaryDto>> GetFilteredPitchesAsync(PitchFilterDto filter)
    {
        var filterBuilder = Builders<Pitch>.Filter;
        var filters = new List<FilterDefinition<Pitch>>
        {
            filterBuilder.Eq(p => p.IsActive, true)
        };

        // Category filter
        if (filter.Category.HasValue)
        {
            filters.Add(filterBuilder.Eq(p => p.Category, filter.Category.Value));
        }

        // Title search
        if (!string.IsNullOrWhiteSpace(filter.SearchTitle))
        {
            filters.Add(filterBuilder.Text(filter.SearchTitle));
        }

        var combinedFilter = filterBuilder.And(filters);

        var pitches = await _context.Pitches
            .Find(combinedFilter)
            .SortByDescending(p => p.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();

        var pitchDtos = new List<PitchSummaryDto>();
        foreach (var pitch in pitches)
        {
            pitchDtos.Add(await MapToPitchSummaryDtoAsync(pitch));
        }

        return pitchDtos;
    }

    public async Task<List<LeaderboardDto>> GetLeaderboardAsync(int limit = 50)
    {
        var pitches = await _context.Pitches
            .Find(p => p.IsActive)
            .ToListAsync();

        var leaderboardItems = new List<LeaderboardDto>();

        foreach (var pitch in pitches)
        {
            var stats = await GetPitchStatsAsync(pitch.Id);
            var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);

            if (team != null)
            {
                var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

                leaderboardItems.Add(new LeaderboardDto
                {
                    Id = pitch.Id,
                    Title = pitch.Title,
                    TeamName = team.Name,
                    TeamLogoUrl = team.LogoUrl,
                    Category = pitch.Category,
                    AverageRating = stats.AverageRating,
                    TotalVotes = stats.TotalVotes,
                    WeightedScore = weightedScore,
                    CreatedAt = pitch.CreatedAt
                });
            }
        }

        // Sort by weighted score descending, then by most recent
        var sortedLeaderboard = leaderboardItems
            .OrderByDescending(l => l.WeightedScore)
            .ThenByDescending(l => l.CreatedAt)
            .Take(limit)
            .ToList();

        // Assign ranks
        for (int i = 0; i < sortedLeaderboard.Count; i++)
        {
            sortedLeaderboard[i].Rank = i + 1;
        }

        return sortedLeaderboard;
    }

    public async Task<PitchDto> UpdatePitchAsync(string pitchId, UpdatePitchDto updatePitchDto, string userId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            throw new ArgumentException("Pitch not found");
        }

        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, pitch.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can update pitches");
        }

        // Update only provided fields
        var updateDefinition = Builders<Pitch>.Update.Set(p => p.UpdatedAt, DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(updatePitchDto.Title))
            updateDefinition = updateDefinition.Set(p => p.Title, updatePitchDto.Title);

        if (!string.IsNullOrWhiteSpace(updatePitchDto.Description))
            updateDefinition = updateDefinition.Set(p => p.Description, updatePitchDto.Description);

        if (updatePitchDto.Category.HasValue)
            updateDefinition = updateDefinition.Set(p => p.Category, updatePitchDto.Category.Value);

        if (updatePitchDto.ProblemStatement != null)
            updateDefinition = updateDefinition.Set(p => p.ProblemStatement, updatePitchDto.ProblemStatement);

        if (updatePitchDto.Solution != null)
            updateDefinition = updateDefinition.Set(p => p.Solution, updatePitchDto.Solution);

        if (updatePitchDto.TargetMarket != null)
            updateDefinition = updateDefinition.Set(p => p.TargetMarket, updatePitchDto.TargetMarket);

        if (updatePitchDto.BusinessModel != null)
            updateDefinition = updateDefinition.Set(p => p.BusinessModel, updatePitchDto.BusinessModel);

        if (updatePitchDto.CompetitiveAdvantage != null)
            updateDefinition = updateDefinition.Set(p => p.CompetitiveAdvantage, updatePitchDto.CompetitiveAdvantage);

        if (updatePitchDto.FundingRequired.HasValue)
            updateDefinition = updateDefinition.Set(p => p.FundingRequired, updatePitchDto.FundingRequired);

        if (updatePitchDto.DemoUrl != null)
            updateDefinition = updateDefinition.Set(p => p.DemoUrl, updatePitchDto.DemoUrl);

        if (updatePitchDto.PitchDeckUrl != null)
            updateDefinition = updateDefinition.Set(p => p.PitchDeckUrl, updatePitchDto.PitchDeckUrl);

        if (updatePitchDto.DemoVideoUrl != null)
            updateDefinition = updateDefinition.Set(p => p.DemoVideoUrl, updatePitchDto.DemoVideoUrl);

        await _context.Pitches.UpdateOneAsync(p => p.Id == pitchId, updateDefinition);

        var updatedPitch = await _context.Pitches
            .Find(p => p.Id == pitchId)
            .FirstOrDefaultAsync();

        return await MapToPitchDtoAsync(updatedPitch!);
    }

    public async Task<bool> DeletePitchAsync(string pitchId, string userId)
    {
        var pitch = await _context.Pitches
            .Find(p => p.Id == pitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            return false;
        }

        // Verify user is a founder of the team
        var isFounder = await _teamService.IsUserFounderOfTeamAsync(userId, pitch.TeamId);
        if (!isFounder)
        {
            throw new UnauthorizedAccessException("Only team founders can delete pitches");
        }

        // Soft delete - mark as inactive
        var updateDefinition = Builders<Pitch>.Update
            .Set(p => p.IsActive, false)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var result = await _context.Pitches.UpdateOneAsync(p => p.Id == pitchId, updateDefinition);
        return result.ModifiedCount > 0;
    }

    public async Task<ReviewStatsDto> GetPitchStatsAsync(string pitchId)
    {
        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .ToListAsync();

        if (!reviews.Any())
        {
            return new ReviewStatsDto
            {
                AverageRating = 0,
                TotalVotes = 0,
                RatingDistribution = new int[5]
            };
        }

        var averageRating = reviews.Average(r => r.Rating);
        var totalVotes = reviews.Count;
        var ratingDistribution = new int[5];

        foreach (var review in reviews)
        {
            ratingDistribution[review.Rating - 1]++; // Rating 1 goes to index 0, etc.
        }

        return new ReviewStatsDto
        {
            AverageRating = Math.Round(averageRating, 2),
            TotalVotes = totalVotes,
            RatingDistribution = ratingDistribution
        };
    }

    public double CalculateWeightedScore(double averageRating, int totalVotes)
    {
        if (totalVotes == 0) return 0;
        
        // Formula: weighted_score = (avg_rating * log10(total_votes + 1))
        return Math.Round(averageRating * Math.Log10(totalVotes + 1), 2);
    }

    private async Task<PitchDto> MapToPitchDtoAsync(Pitch pitch)
    {
        var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);
        var stats = await GetPitchStatsAsync(pitch.Id);
        var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

        return new PitchDto
        {
            Id = pitch.Id,
            TeamId = pitch.TeamId,
            Title = pitch.Title,
            Description = pitch.Description,
            Category = pitch.Category,
            ProblemStatement = pitch.ProblemStatement,
            Solution = pitch.Solution,
            TargetMarket = pitch.TargetMarket,
            BusinessModel = pitch.BusinessModel,
            CompetitiveAdvantage = pitch.CompetitiveAdvantage,
            FundingRequired = pitch.FundingRequired,
            DemoUrl = pitch.DemoUrl,
            PitchDeckUrl = pitch.PitchDeckUrl,
            DemoVideoUrl = pitch.DemoVideoUrl,
            CreatedAt = pitch.CreatedAt,
            Team = team ?? new TeamDto(),
            AverageRating = stats.AverageRating,
            TotalVotes = stats.TotalVotes,
            WeightedScore = weightedScore
        };
    }

    private async Task<PitchSummaryDto> MapToPitchSummaryDtoAsync(Pitch pitch)
    {
        var team = await _teamService.GetTeamByIdAsync(pitch.TeamId);
        var stats = await GetPitchStatsAsync(pitch.Id);
        var weightedScore = CalculateWeightedScore(stats.AverageRating, stats.TotalVotes);

        return new PitchSummaryDto
        {
            Id = pitch.Id,
            Title = pitch.Title,
            Description = pitch.Description,
            Category = pitch.Category,
            CreatedAt = pitch.CreatedAt,
            Team = team ?? new TeamDto(),
            AverageRating = stats.AverageRating,
            TotalVotes = stats.TotalVotes,
            WeightedScore = weightedScore
        };
    }
}
```

Now let me create the Review service:

```csharp
using Backend.DTOs;

namespace Backend.Services;

public interface IReviewService
{
    Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto, string reviewerId);
    Task<PaginatedReviewsDto> GetPitchReviewsAsync(string pitchId, int page = 1, int pageSize = 5);
    Task<List<ReviewDto>> GetLatestReviewsForPitchAsync(string pitchId, int count = 5);
    Task<ReviewDto?> GetUserReviewForPitchAsync(string userId, string pitchId);
    Task<bool> HasUserReviewedPitchAsync(string userId, string pitchId);
    Task<List<ReviewDto>> GetReviewsByUserAsync(string userId);
}
```

```csharp
using Backend.Data;
using Backend.DTOs;
using Backend.Entities;
using MongoDB.Driver;

namespace Backend.Services;

public class ReviewService : IReviewService
{
    private readonly MongoDbContext _context;
    private readonly IAuthService _authService;

    public ReviewService(MongoDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<ReviewDto> CreateReviewAsync(CreateReviewDto createReviewDto, string reviewerId)
    {
        // Verify reviewer exists and is a reviewer
        var reviewer = await _authService.GetUserByIdAsync(reviewerId);
        if (reviewer == null || reviewer.Role != UserRole.Reviewer)
        {
            throw new UnauthorizedAccessException("Only reviewers can create reviews");
        }

        // Verify pitch exists
        var pitch = await _context.Pitches
            .Find(p => p.Id == createReviewDto.PitchId && p.IsActive)
            .FirstOrDefaultAsync();

        if (pitch == null)
        {
            throw new ArgumentException("Pitch not found");
        }

        // Check if user has already reviewed this pitch
        var existingReview = await _context.Reviews
            .Find(r => r.PitchId == createReviewDto.PitchId && r.ReviewerId == reviewerId)
            .FirstOrDefaultAsync();

        if (existingReview != null)
        {
            throw new InvalidOperationException("You have already reviewed this pitch");
        }

        // Prevent reviewers from reviewing their own team's pitches
        var team = await _context.Teams
            .Find(t => t.Id == pitch.TeamId && t.IsActive)
            .FirstOrDefaultAsync();

        if (team != null && team.FounderIds.Contains(reviewerId))
        {
            throw new UnauthorizedAccessException("You cannot review your own team's pitch");
        }

        var review = new Review
        {
            PitchId = createReviewDto.PitchId,
            ReviewerId = reviewerId,
            Rating = createReviewDto.Rating,
            Feedback = createReviewDto.Feedback,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Reviews.InsertOneAsync(review);
        return await MapToReviewDtoAsync(review);
    }

    public async Task<PaginatedReviewsDto> GetPitchReviewsAsync(string pitchId, int page = 1, int pageSize = 5)
    {
        var totalCount = (int)await _context.Reviews
            .CountDocumentsAsync(r => r.PitchId == pitchId);

        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .SortByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return new PaginatedReviewsDto
        {
            Reviews = reviewDtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        };
    }

    public async Task<List<ReviewDto>> GetLatestReviewsForPitchAsync(string pitchId, int count = 5)
    {
        var reviews = await _context.Reviews
            .Find(r => r.PitchId == pitchId)
            .SortByDescending(r => r.CreatedAt)
            .Limit(count)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return reviewDtos;
    }

    public async Task<ReviewDto?> GetUserReviewForPitchAsync(string userId, string pitchId)
    {
        var review = await _context.Reviews
            .Find(r => r.ReviewerId == userId && r.PitchId == pitchId)
            .FirstOrDefaultAsync();

        return review != null ? await MapToReviewDtoAsync(review) : null;
    }

    public async Task<bool> HasUserReviewedPitchAsync(string userId, string pitchId)
    {
        var count = await _context.Reviews
            .CountDocumentsAsync(r => r.ReviewerId == userId && r.PitchId == pitchId);

        return count > 0;
    }

    public async Task<List<ReviewDto>> GetReviewsByUserAsync(string userId)
    {
        var reviews = await _context.Reviews
            .Find(r => r.ReviewerId == userId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();

        var reviewDtos = new List<ReviewDto>();
        foreach (var review in reviews)
        {
            reviewDtos.Add(await MapToReviewDtoAsync(review));
        }

        return reviewDtos;
    }

    private async Task<ReviewDto> MapToReviewDtoAsync(Review review)
    {
        var reviewer = await _authService.GetUserByIdAsync(review.ReviewerId);

        return new ReviewDto
        {
            Id = review.Id,
            PitchId = review.PitchId,
            ReviewerId = review.ReviewerId,
            ReviewerName = reviewer != null ? $"{reviewer.FirstName} {reviewer.LastName}" : "Unknown Reviewer",
            Rating = review.Rating,
            Feedback = review.Feedback,
            CreatedAt = review.CreatedAt
        };
    }
}
```

Now let me update the Program.cs to configure all the services and JWT authentication:

```csharp
using Backend.Configuration;
using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Configure JWT
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// Register MongoDB context
builder.Services.AddSingleton<MongoDbContext>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPitchService, PitchService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "")),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings?.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Pitch Showcase API", Version = "v1" });
    
    // Configure JWT in Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Initialize MongoDB indexes
using (var scope = app.Services.CreateScope())
{
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await mongoContext.CreateIndexesAsync();
}

app.Run();
```

Now let me update the appsettings.json with the configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "PitchShowcaseDB",
    "UsersCollectionName": "users",
    "TeamsCollectionName": "teams",
    "PitchesCollectionName": "pitches",
    "ReviewsCollectionName": "reviews"
  },
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForSecurityPurposes",
    "Issuer": "PitchShowcaseAPI",
    "Audience": "PitchShowcaseUsers",
    "ExpirationInMinutes": 1440
  }
}
```

Perfect! I've completed the comprehensive backend services layer. Let me summarize what we've built so far:

## ✅ **Completed Backend Services Layer:**

### **🔐 Authentication Service (`AuthService`)**
- User registration with password hashing (BCrypt)
- JWT token generation and validation  
- Login with email/password verification
- Role-based authentication (Founder/Reviewer)

### **👥 Team Service (`TeamService`)**
- Create teams with multiple founders
- Founder validation and authorization
- Team updates (only by founders)
- Soft delete functionality
- Team listing with pitch counts

### **🎯 Pitch Service (`PitchService`)**
- Create pitches (only by team founders)
- **Weighted scoring algorithm**: `(avg_rating * log10(total_votes + 1))`
- Category filtering (FinTech/HealthTech/AI/Other)
- Title search functionality
- Leaderboard generation with ranking
- Comprehensive pitch details with stats

### **⭐ Review Service (`ReviewService`)**
- Create reviews (only by reviewers, 1-5 scale)
- **No duplicate reviews** (enforced at DB and service level)
- **No self-reviews** (founders can't review own pitches)
- Pagination support (latest 5 feedback + full pagination)
- Review statistics and distribution

### **🔧 Configuration & Infrastructure**
- MongoDB context with proper indexing
- JWT authentication middleware
- CORS configuration for Angular frontend
- Swagger documentation with JWT support
- Dependency injection setup

## **🎯 Key Features Implemented:**

✅ **Weighted Scoring Formula** - Exactly as requested  
✅ **Role-Based Authorization** - Founders vs Reviewers  
✅ **Multiple Founders per Team** - No limit on team size  
✅ **Category Filtering** - FinTech/HealthTech/AI/Other  
✅ **Title Search** - MongoDB text search  
✅ **Pagination** - For reviews and listings  
✅ **No Rating Updates** - Reviews are immutable  
✅ **Comprehensive Validation** - Business rules enforced  

## **Next Steps:**

Would you like me to continue with:

1. **🎮 Controllers Layer** - REST API endpoints for all operations
2. **📊 Mock Data Generation** - 80+ teams and 120+ pitches  
3. **🚀 AWS Lambda Configuration** - Serverless deployment setup
4. **🅰️ Angular Frontend** - Move to frontend development

Which would you prefer to tackle next? I recommend creating the Controllers first to complete the backend API, then we can test it before moving to the frontend.

