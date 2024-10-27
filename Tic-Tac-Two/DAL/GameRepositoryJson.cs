using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    
    
    public void SaveGame(string jsonStateString, string gameConfigName)
    {
        var filename = FileHandler.BasePath 
                       + gameConfigName + " " + DateTime.Now.ToString("O") 
                       + FileHandler.GameExtension;
        System.IO.File.WriteAllText(filename, jsonStateString);
    }
}