using GovServices.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace GovServices.Server.Services;

public interface IDictionaryCacheService
{
    string? GetValue(string dictionaryName, string key);
    Task ReloadAsync();
}

public class DictionaryCacheService : IDictionaryCacheService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Dictionary<string, Dictionary<string, string>> _cache = new();

    public DictionaryCacheService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task ReloadAsync()
    {
        _cache.Clear();
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var dicts = await context.Dictionaries.ToListAsync();
        foreach (var d in dicts)
        {
            try
            {
                var arr = JArray.Parse(d.Data);
                var map = new Dictionary<string, string>();
                foreach (var item in arr)
                {
                    var props = item.Children<JProperty>().ToList();
                    if (props.Count >= 2)
                        map[props[0].Value.ToString()] = props[1].Value.ToString();
                }
                _cache[d.Name] = map;
            }
            catch { }
        }
    }

    public string? GetValue(string dictionaryName, string key)
    {
        if (_cache.TryGetValue(dictionaryName, out var map))
        {
            if (map.TryGetValue(key, out var value))
                return value;
        }
        return null;
    }
}
