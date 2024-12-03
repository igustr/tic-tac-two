using DAL;
using MenuSystem;

namespace ConsoleApp;

public class LoadGameController
{
    private static IGameRepository _gameRepository = default!;
    
    public static string ChooseConfigurationLoadGame(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
        
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < gameRepository.GetSavedGamesNames().Count; i++)
        {
            var titleString = gameRepository.GetSavedGamesNames()[i];
            var title = titleString.Split(" ");
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = title[0],
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

        GameController.MainLoop(chosenConfig, "load", configRepository, gameRepository);
        return "load";
    }
}