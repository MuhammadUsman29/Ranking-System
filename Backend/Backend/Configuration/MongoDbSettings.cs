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