using Domain;
using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    private const string Filter = "*";
    
    public int SaveGame(string jsonStateString, int gameId, string userGameName)
    {
        if (!Directory.Exists(FileHandler.BasePath))
        {
            Directory.CreateDirectory(FileHandler.BasePath);
        }
        
        var filename = FileHandler.BasePath 
                       + userGameName + " " + DateTime.Now.ToString("yyyy.MM.dd.T.HH.mm.ss") 
                       + FileHandler.GameExtension;
        
        System.IO.File.WriteAllText(filename, jsonStateString);
        return -1;
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
    
    public GameState GetSavedGameByName(string name)
    {
        var configJsonStr = System.IO.File.ReadAllText(
            FileHandler.BasePath + name + FileHandler.GameExtension);
        var gameState = System.Text.Json.JsonSerializer.Deserialize<GameState>(configJsonStr);
        // Console.WriteLine("game state: " + gameState);
        return gameState;
    }
    
    public GameState LoadGame(int gameId)
    {
        throw new NotImplementedException();
    }

    public int GetIdByName(string name)
    {
        throw new NotImplementedException();
    }

    public void DeleteGame(int gameId)
    {
        throw new NotImplementedException();
    }
}