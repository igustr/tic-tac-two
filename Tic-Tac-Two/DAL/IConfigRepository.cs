using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
    public List<string> GetSavedGamesNames();
    public GameState GetSavedConfigurationByName(string name);
    public void SaveConfig(string jsonConfigString);
}