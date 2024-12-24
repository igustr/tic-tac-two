using Domain;
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
    
    public void SaveConfig(string jsonConfigString)
    {
        var userConfigName = "";
        
        if (!Directory.Exists(FileHandler.BasePath))
        {
            Directory.CreateDirectory(FileHandler.BasePath);
        }
        
        var filename = FileHandler.BasePath 
                       + userConfigName + ".config" + FileHandler.GameExtension;
        
        System.IO.File.WriteAllText(filename, jsonConfigString);
    }
    
    public GameState GetSavedConfigurationByName(string name)
    {
        Console.WriteLine("name: " + name);
        var configJsonStr = System.IO.File.ReadAllText(
            FileHandler.BasePath + name + FileHandler.GameExtension);
        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(configJsonStr);
        
        return gameState;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
    
    public GameConfiguration GetConfigById(int configId)
    {
        throw new NotImplementedException(); 
    }
    public List<Configuration> GetConfigurations()
    {
        throw new NotImplementedException(); 
    }
}