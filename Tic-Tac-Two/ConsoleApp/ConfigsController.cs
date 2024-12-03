using DAL;
using MenuSystem;

namespace ConsoleApp;

public static class ConfigsController
{
    private static IConfigRepository _configRepository = default!;
    
    public static string ChooseSavedConfiguration(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < _configRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = _configRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }
        
        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        var chosenConfig = configMenu.Run();

        GameController.MainLoop(chosenConfig, "new", configRepository, gameRepository);
        return "new";
    }
    
    
}