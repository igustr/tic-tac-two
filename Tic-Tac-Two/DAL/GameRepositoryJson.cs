using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    public void SaveGame(string jsonStateString, string gameConfigName, string gameName)
    {
        if (!Directory.Exists(FileHandler.BasePath))
        {
            Directory.CreateDirectory(FileHandler.BasePath);
        }
        
        var filename = FileHandler.BasePath 
                       + gameConfigName + "_" + gameName + " " + DateTime.Now.ToString("yyyy.MM.dd.T.HH.mm.ss") 
                       + FileHandler.GameExtension;
        
        System.IO.File.WriteAllText(filename, jsonStateString);
    }
}