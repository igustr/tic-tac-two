using GameBrain;

namespace DAL;

public class ConfigRepositoryJson : IConfigRepository
{
    private readonly string _basePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "Tic-Tac-Two"
    ) + Path.DirectorySeparatorChar;
    
    public List<string> GetConfigurationNames()
    {
        Console.WriteLine("PATH: " + _basePath);
        
        _checkAndCreatInitialConfig();

        var res = new List<string>();
        foreach (var fullFileName in System.IO.Directory.GetFiles(_basePath, "*.config.json"))
        {
            var filenameParts = System.IO.Path.GetFileNameWithoutExtension(fullFileName);
            var primaryName = System.IO.Path.GetFileNameWithoutExtension(filenameParts);
            res.Add(primaryName);
        }
        return res;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(_basePath + name + ".config.json");
        var config = System.Text.Json.JsonSerializer.Deserialize<GameConfiguration>(configJsonStr);
        return config;

    }

    private void _checkAndCreatInitialConfig()
    {
        if (!System.IO.Directory.Exists(_basePath))
        {
            System.IO.Directory.CreateDirectory(_basePath);
        }
        var data = System.IO.Directory.GetFiles(_basePath, "*.config.json").ToList();
        if (data.Count == 0)
        {
            var hardcodedRepo = new ConfigRepositoryHardcoded();
            var optionNames = hardcodedRepo.GetConfigurationNames();
            foreach (var optionName in optionNames)
            {
                var gameOption = hardcodedRepo.GetConfigurationByName(optionName);
                var optionJsonStr = System.Text.Json.JsonSerializer.Serialize(gameOption);
                Console.WriteLine("HERE" + optionJsonStr);
                System.IO.File.WriteAllText(
                    Path.Combine(_basePath, gameOption.Name + ".config.json"), 
                    optionJsonStr
                );

            }
        }
    }
}