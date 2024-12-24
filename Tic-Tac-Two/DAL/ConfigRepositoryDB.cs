using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL;

public class ConfigRepositoryDB : IConfigRepository
{
    private readonly DAL.AppDbContext _context;
    
    public ConfigRepositoryDB(AppDbContext context)
    {
        _context = context;
    }
    
    public List<string> GetConfigurationNames()
    {
        var configNames = _context.Configurations
            .OrderBy(c => c.Name)
            .Select(c => c.Name)
            .ToList();

        return configNames;
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        //Console.WriteLine("name: " + name);
        var data = _context.Configurations.First(c => c.Name == name);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Configuration with name '{name}' not found.");
        }
        
        var res = new GameConfiguration()
        {
            Name = data.Name,
            BoardSizeWidth = data.BoardSizeWidth,
            BoardSizeHeight = data.BoardSizeHeight,
            GridSizeHeight = data.GridSizeHeight,
            GridSizeWidth = data.GridSizeWidth,
            WinCondition = data.WinCondition,
            MovePieceAfterNMoves = data.MovePieceAfterNMoves,
            AmountOfPieces = data.AmountOfPieces
        };
        return res;
    }

    public int SaveConfig(string jsonConfigString, string userConfigName)
    {
        // Deserialize the JSON string to a GameConfiguration object
        var config = JsonSerializer.Deserialize<GameConfiguration>(jsonConfigString);

        if (config == null)
        {
            throw new InvalidOperationException("Failed to deserialize configuration.");
        }

        // Create a new ConfigEntity from GameConfiguration
        var configEntity = new Configuration
        {
            Name = userConfigName,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            GridSizeWidth = config.GridSizeWidth,
            GridSizeHeight = config.GridSizeHeight,
            WinCondition = config.WinCondition,
            MovePieceAfterNMoves = config.MovePieceAfterNMoves,
            AmountOfPieces = config.AmountOfPieces
        };
        
        _context.Configurations.Add(configEntity);
        
        _context.SaveChanges();

        return configEntity.Id;
    }
    
    public GameConfiguration GetConfigById(int configId)
    {
        var data = _context.Configurations.First(g => g.Id == configId);
        
        if (data == null)
        {
            // Log or throw a custom exception if needed
            throw new KeyNotFoundException($"Configuration with id '{configId}' not found.");
        }
        var res = new GameConfiguration()
        {
            Name = data.Name,
            BoardSizeWidth = data.BoardSizeWidth,
            BoardSizeHeight = data.BoardSizeHeight,
            GridSizeHeight = data.GridSizeHeight,
            GridSizeWidth = data.GridSizeWidth,
            WinCondition = data.WinCondition,
            MovePieceAfterNMoves = data.MovePieceAfterNMoves,
            AmountOfPieces = data.AmountOfPieces
        };
        return res;
    }
    
    public List<Configuration> GetConfigurations()
    {
        var configurations = _context.Configurations
            .OrderBy(c => c.Name)
            .ToList();

        return configurations;
    }

}