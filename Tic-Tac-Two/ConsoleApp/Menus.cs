using MenuSystem;

namespace ConsoleApp;

public static class Menus
{
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

    public static Menu MainMenu = new Menu(
        EMenuLevel.Main,
        "TIC-TAC-TOE", [
            new MenuItem()
            {
                Shortcut = "N",
                Title = "New game",
                MenuItemAction = GameController.MainLoop
            },
            new MenuItem()
            {
                Shortcut = "L",
                Title = "Load game",
                MenuItemAction = GameController.MainLoop
            },
            new MenuItem()
            {
            Shortcut = "O",
            Title = "Options",
            MenuItemAction = OptionsMenu.Run
            },
        ]);
    
    /*
    public static Menu GameOptionsMenu = new Menu(
        EMenuLevel.Main,
        "OPTIONS MENU", [
            new MenuItem()
            {
                Shortcut = "B",
                Title = "Back",
                MenuItemAction = GameController.MainLoop
            },
            new MenuItem()
            {
                Shortcut = "S",
                Title = "Save",
                MenuItemAction = OptionsMenu.Run
            },            
            new MenuItem()
            {
                Shortcut = "E",
                Title = "Exit",
                MenuItemAction = OptionsMenu.Run
            },
        ]);
        */

    private static string DummyMethod()
    {
        Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
        Console.ReadKey();
        return "foobar";
    }
}