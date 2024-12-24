using Domain;
using GameBrain;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration GetConfigurationByName(string name);
    public int SaveConfig(string jsonConfigString, string userConfigName);
    public GameConfiguration GetConfigById(int configId);
    public List<Configuration> GetConfigurations();
}