using GameBrain;

namespace DAL;

public class GameRepositoryJson : IGameRepository
{
    private const string Filter = "*";
    
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

    public List<string> LoadGame()
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
}