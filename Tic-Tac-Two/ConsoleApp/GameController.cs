using System.Globalization;
using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryJson();
    private static readonly IGameRepository GameRepository = new GameRepositoryJson();
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
            Console.Clear();
            Console.WriteLine();
            Visualizer.DrawGame(gameInstance);
            if (gameInstance.CheckWin())
            {
                //Todo return to menu where you can choose to continue or return
                return "Winner is X";
                // Implement start new game button
            }

            gameInstance.CheckWin();
            Console.WriteLine("Win? : " + gameInstance.CheckWin());
            
            if (_invalidInput)
            {
                Console.WriteLine();
                Console.WriteLine("\u001b[31mInvalid input!\u001b[0m");
            }
            
            Console.WriteLine();
            Console.WriteLine("1) Type <x,y> - Insert coordinates: ");
            Console.WriteLine("G) Move grid: ");
            Console.WriteLine("O) Options: ");
            Console.Write("> ");
            
            var input = Console.ReadLine()!;
            _invalidInput = false;
            
            if (input.Equals("G", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                Visualizer.DrawGame(gameInstance);
                gameInstance.MoveGrid();
            } else if (input.Equals("O", StringComparison.CurrentCultureIgnoreCase))
            {
                GameOptionsMenu(gameInstance);
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

    private static void GameOptionsMenu(TicTacTwoBrain gameInstance)
    {
        Console.Clear();
        Console.WriteLine("GAME OPTIONS");
        Console.WriteLine("============");
        Console.WriteLine("B) Back");
        Console.WriteLine("S) Save");
        Console.WriteLine("R) Restart");
        Console.WriteLine("E) Exit");
        
        Console.Write("> ");
        var input = Console.ReadLine()!;

        switch (input)
        {
            case "B":
                break;
            case "R":
                gameInstance.ResetGame();
                break;
            case "S":
                GameRepository.SaveGame(gameInstance.GetGameStateJson(), gameInstance.GetGameConfigName());
                break;
            case "E":
                gameInstance.ExitGame();
                break;
            default:
                Console.WriteLine("\u001b[31mInvalid input!\u001b[0m");
                Console.WriteLine("B) Back");
                Console.WriteLine("S) Save");
                Console.WriteLine("R) Restart");
                Console.WriteLine("E) Exit");
                break;
        }
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