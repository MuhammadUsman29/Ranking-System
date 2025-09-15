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