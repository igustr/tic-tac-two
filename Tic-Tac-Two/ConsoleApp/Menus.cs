using MenuSystem;
using DAL;

namespace ConsoleApp;

public static class Menus
{
    private static IConfigRepository _configRepository = default!;
    private static IGameRepository _gameRepository = default!;

    public static void Init(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }
    
    public static readonly Menu DeepMenu = new Menu(
        EMenuLevel.Deep,
        "TIC-TAC-TOE DEEP", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "NOPE",
            }
        ]
    );

    public static readonly Menu OptionsMenu =
        new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TOE Options", [
                new MenuItem()
                {
                    Shortcut = "X",
                    Title = "X Starts",
                    MenuItemAction = DeepMenu.Run
                },
                new MenuItem()
                {
                    Shortcut = "O",
                    Title = "O Starts",
                    MenuItemAction = DummyMethod
                },
            ]);

    public static readonly Menu NewGameMenu = new Menu(
        EMenuLevel.Secondary,
        "TIC-TAC-TOE Games", [
            new MenuItem()
            {
                Shortcut = "D",
                Title = "Default",
                MenuItemAction = () => GameController.MainLoop("1","new", 
                    _configRepository, _gameRepository)
            },
            new MenuItem()
            {
                Shortcut = "C",
                Title = "Custom",
                MenuItemAction = () => GameController.MainLoop("2","new", 
                    _configRepository, _gameRepository)
            },
        ]);
    
    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "TIC-TAC-TOE", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "New game",
                MenuItemAction = NewGameMenu.Run
            },
            new MenuItem()
            {
                Shortcut = "L",
                Title = "Load game",
                MenuItemAction = () => LoadGameController.ChooseConfigurationLoadGame(_configRepository, _gameRepository)
            },
            new MenuItem()
            {
                Shortcut = "C",
                Title = "Configurations",
                MenuItemAction = () => ConfigsController.ChooseSavedConfiguration(_configRepository, _gameRepository)
            },
            new MenuItem()
            {
                Shortcut = "O",
                Title = "Options",
                MenuItemAction = OptionsMenu.Run
            },
        ]);

    
    private static string DummyMethod()
    {
        Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
        Console.ReadKey();
        return "foobar";
    }
}