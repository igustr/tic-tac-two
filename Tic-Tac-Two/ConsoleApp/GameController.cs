using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly ConfigRepository ConfigRepository = new ConfigRepository();
    private static bool _invalidInput = false;
    
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
        
        do
        {
            Console.WriteLine();
            Visualizer.DrawGame(gameInstance);
            gameInstance.CheckWin();
            if (_invalidInput)
            {
                Console.WriteLine();
                Console.WriteLine("\u001b[31mInvalid input!\u001b[0m");
            }
            Console.WriteLine();
            Console.WriteLine("Press G to move grid: ");
            Console.WriteLine("Insert coordinates <x,y>: ");
            Console.Write("> ");
            var input = Console.ReadLine()!;
            _invalidInput = false;
            
            if (input.Equals("G", StringComparison.CurrentCultureIgnoreCase))
            {
                gameInstance.MoveGrid();
            }
            else
            {
                var inputSplit = input.Split(",");

                if (inputSplit.Length != 2)
                {
                    _invalidInput = true;
                    continue; 
                }
                
                if (int.TryParse(inputSplit[0], out var inputX) && int.TryParse(inputSplit[1], out var inputY))
                {
                    gameInstance.MakeAMove(inputX - 1, inputY - 1); ;
                }
                else
                {
                    _invalidInput = true;
                }
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