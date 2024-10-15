using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly ConfigRepository ConfigRepository = new ConfigRepository();
    private static string _direction;
    
    public static string MainLoop()
    {
        var chosenConfigShortcut = ChooseConfiguration();
        

        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            return chosenConfigShortcut;
        }

        var chosenConfig = ConfigRepository.GetConfigurationByName(
            ConfigRepository.GetConfigurationNames()[configNo]
        );

        if (chosenConfig.Name == "Custom")
        {
            chosenConfig.CustomGameCheck();
        }
        
        var gameInstance = new TicTacTwoBrain(chosenConfig);


        // main loop of gameplay
        // draw the board again
        // ask input again, validate input
        // is game over?

        //TODO Wrong input implementation
        
        do
        {
            Visualizer.DrawGame(gameInstance);
            Console.WriteLine();
            Console.WriteLine("Press G to move grid: ");
            Console.WriteLine("Insert coordinates <x,y>: ");
            Console.Write("> ");
            var input = Console.ReadLine()!;
            if (input.Equals("G", StringComparison.CurrentCultureIgnoreCase))
            {
                gameInstance.MoveGrid();
            }
            else
            {
                var inputSplit = input.Split(",");
                var inputX = int.Parse(inputSplit[0]);
                var inputY = int.Parse(inputSplit[1]);
                gameInstance.MakeAMove(inputX - 1, inputY - 1);
            }
        } while (true);
    }
    

    private static string ChooseConfiguration()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetConfigurationNames().Count; i++)
        {
            var returnValue = i.ToString();
            configMenuItems.Add(new MenuItem()
            {
                Title = ConfigRepository.GetConfigurationNames()[i],
                Shortcut = (i + 1).ToString(),
                MenuItemAction = () => returnValue
            });
        }

        var configMenu = new Menu(EMenuLevel.Secondary,
            "TIC-TAC-TOE - choose game config",
            configMenuItems,
            isCustomMenu: true
        );

        return configMenu.Run();
    }
}