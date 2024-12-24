using Domain;
using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
    public void SaveConfig(string jsonConfigString);
    public GameConfiguration GetConfigById(int configId);
    public List<Configuration> GetConfigurations();
}