using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data;

public class RedisPlatformRepository : IPlatformRepository
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisPlatformRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public void CreatePlatform(Platform platform)
    {
        string serializedPlatform = JsonSerializer.Serialize(platform);

        _database.HashSet("hashplatform", new HashEntry[]{
                new HashEntry(platform.Id, serializedPlatform)
                });
    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        HashEntry[]? hashes = _database.HashGetAll("hashplatform");

        List<Platform?>? result = Array.ConvertAll(hashes, entry => JsonSerializer.Deserialize<Platform>(entry.Value)).ToList();

        return result;
    }

    public Platform? GetPlatformById(string id)
    {
        string platform = _database.HashGet("hashplatform", id);

        return platform == null
            ? null
            : JsonSerializer.Deserialize<Platform>(platform);
    }
}
