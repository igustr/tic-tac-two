using GameBrain;

namespace DAL;

public interface IGameRepository
{
    public void SaveGame(string jsonStateString, string gameConfigName, string gameName);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedGame(string name);
}