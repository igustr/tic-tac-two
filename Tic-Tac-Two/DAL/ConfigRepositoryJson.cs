using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    private const string Filter = "*";

    public List<string> GetConfigurationNames()
    {
        //Console.WriteLine("PATH: " + FileHandler.BasePath);
        
        _checkAndCreatInitialConfig();

        var res = new List<string>();
        foreach (var fullFileName in System.IO.Directory.GetFiles(
                     FileHandler.BasePath, Filter + FileHandler.ConfigExtension))
        {
            var filenameParts = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            var primaryName = System.IO.Path.GetFileNameWithoutExtension(filenameParts);
            res.Add(primaryName);
        }
        return res;
    }
    

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(
            FileHandler.BasePath + name + FileHandler.ConfigExtension);
        var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;

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
    
    public GameState GetSavedConfigurationByName(string name)
    {
        Console.WriteLine("name: " + name);
        var configJsonStr = System.IO.File.ReadAllText(
            FileHandler.BasePath + name + FileHandler.GameExtension);
        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(configJsonStr);
        
        return gameState;
    }

    private void _checkAndCreatInitialConfig()
    {
        if (!System.IO.Directory.Exists(FileHandler.BasePath))
        {
            System.IO.Directory.CreateDirectory(FileHandler.BasePath);
        }
        
        var data = System.IO.Directory.GetFiles(
            FileHandler.BasePath, Filter + FileHandler.ConfigExtension).ToList();
        
        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var optionNames = hardcodedRepo.GetConfigurationNames();
            foreach (var optionName in optionNames)
            {
                var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameOption);
                System.IO.File.WriteAllText(
                    Path.Combine(FileHandler.BasePath, gameOption.Name + FileHandler.ConfigExtension), 
                    optionJsonStr
                );

            }
        }
    }
}