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
    
    public void SaveConfig(string jsonConfigString)
    {
        Console.WriteLine("Type configuration name: ");
        Console.Write("> ");
        var userConfigName = Console.ReadLine()!;
        
        if (!Directory.Exists(FileHandler.BasePath))
        {
            Directory.CreateDirectory(FileHandler.BasePath);
        }
        
        var filename = FileHandler.BasePath 
                       + userConfigName + ".config" + FileHandler.GameExtension;
        
        System.IO.File.WriteAllText(filename, jsonConfigString);
        Console.WriteLine("Configuration Saved!");
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