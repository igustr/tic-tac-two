using GameBrain;

namespace DAL;

public class ConfigRepositoryHardcoded : IConfigRepository
{

    private const string Filter = "*";
    
    private List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration()
        {
            Name = "Classical",
        },
        new GameConfiguration()
        {
            Name = "Custom",
        },
    };
    
    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }
    
    public List<string> GetSavedGamesNames()
    {
        var res = new List<string>();
        foreach (var fullFileName in System.IO.Directory.GetFiles(
                     FileHandler.BasePath, Filter + FileHandler.GameExtension))
        {
            var filenameParts = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            var primaryName = System.IO.Path.GetFileNameWithoutExtension(filenameParts);
            res.Add(primaryName);
        }
        return res;
    }
    
    public GameConfiguration GetSavedConfigurationByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(
            FileHandler.BasePath + name + FileHandler.ConfigExtension);
        var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
}