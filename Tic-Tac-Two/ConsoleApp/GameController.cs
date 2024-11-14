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
    private static bool _invalidInput;
    private static bool _invalidMove;
    public static int AmountOfPiecesX = 0;
    public static int AmountOfPiecesO = 0;
    private static int _movePieceAfterNMoves = 2;
    
    
    public static string MainLoop(string chosenConfigShortcut, string gameType)
    {
        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            return chosenConfigShortcut;
        }

        GameConfiguration chosenConfig;
        TicTacTwoBrain gameInstance;
        
        if (gameType == "load")
        {
            var gameState = ConfigRepository.GetSavedConfigurationByName(
                ConfigRepository.GetSavedGamesNames()[configNo]
            );
            chosenConfig = gameState.GameConfiguration;
    
            gameInstance = new TicTacTwoBrain(gameState);

            gameInstance.LoadGame(gameState);
        } else 
        {
            chosenConfig = ConfigRepository.GetConfigurationByName(
                ConfigRepository.GetConfigurationNames()[configNo]
            );
            if (chosenConfig.Name == "Custom")
            {
                chosenConfig.CustomGameCheck();
                _movePieceAfterNMoves = chosenConfig.MovePieceAfterNMoves;
            } 
            
            gameInstance = new TicTacTwoBrain(chosenConfig);
        }
        
        
        //var gameInstance = new TicTacTwoBrain(chosenConfig);
        
        do
        {
            //Console.Clear();
            Visualizer.DrawGame(gameInstance); 
            Console.WriteLine(InputCheck());
            _invalidInput = false;
            _invalidMove = false;
            if (gameInstance.CheckWin())
            {
                //Todo return to menu where you can choose to continue or return
                return "Winner is X";
                // Implement start new game button
            }

            //gameInstance.CheckWin();
            //Console.WriteLine("Win? : " + gameInstance.CheckWin());
            //Console.WriteLine("X " + AmountOfPiecesX);
            //Console.WriteLine("O " + AmountOfPiecesO);
            //Console.WriteLine("MovePieceAfterNMoves " + _movePieceAfterNMoves);
            //Console.WriteLine("Next Move By " + gameInstance.NextMoveBy);
            
            // Player X Turn
            if (gameInstance.NextMoveBy == EGamePiece.X)
            {
                if (AmountOfPiecesX < _movePieceAfterNMoves)
                {
                    FirstLevel(gameInstance);
                }
                else if (AmountOfPiecesX == chosenConfig.AmountOfPieces)
                {
                    ThirdLevel(gameInstance);
                }
                else if (AmountOfPiecesX >= _movePieceAfterNMoves)
                {
                    SecondLevel(gameInstance);
                }

            } 
            // Player O Turn
            else if (gameInstance.NextMoveBy == EGamePiece.O)
            {
                if (AmountOfPiecesO < _movePieceAfterNMoves)
                {
                    FirstLevel(gameInstance);
                }
                else if (AmountOfPiecesO == chosenConfig.AmountOfPieces)
                {
                    ThirdLevel(gameInstance);
                }
                else if (AmountOfPiecesO >= _movePieceAfterNMoves)
                {
                    SecondLevel(gameInstance);
                }
            }
            
            
        } while (true);
    }
    
    
    private static void FirstLevel(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine();
        Console.WriteLine("1) Type <x,y> - Insert coordinates");
        Console.WriteLine("O) Options: ");
        Console.Write("> ");
        var input = Console.ReadLine()!;

        if (input.Equals("O", StringComparison.CurrentCultureIgnoreCase))
        {
            GameOptionsMenu(gameInstance);
        }
        else
        {
            InsertCoordinates(gameInstance, input);
        }
    }

    private static void SecondLevel(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine();
        Console.WriteLine("1) Type <x,y> - Insert coordinates");
        Console.WriteLine("M) Move Piece");
        Console.WriteLine("G) Move grid");
        Console.WriteLine("O) Options");
        Console.Write("> ");
        var input = Console.ReadLine()!;
        
        if (input.Equals("G", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.Clear();
            Visualizer.DrawGame(gameInstance);
            gameInstance.MoveGrid();
        } else if (input.Equals("O", StringComparison.CurrentCultureIgnoreCase))
        {
            GameOptionsMenu(gameInstance);
        } else if (input.Equals("M", StringComparison.CurrentCultureIgnoreCase))
        {
            bool moveSuccessful;
            do
            {
                moveSuccessful = gameInstance.MovePiece();
            } while (!moveSuccessful);
            Console.WriteLine("Type <x,y> - Insert new coordinates");
            Console.Write("> ");
            var newInput = Console.ReadLine()!;
            InsertCoordinates(gameInstance, newInput);
        }
        else
        {
            InsertCoordinates(gameInstance, input);
        }
    }

    private static void ThirdLevel(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine();
        Console.WriteLine("M) Move Piece");
        Console.WriteLine("G) Move grid");
        Console.WriteLine("O) Options");
        Console.Write("> ");
        
        var input = Console.ReadLine()!;
        
        if (input.Equals("G", StringComparison.CurrentCultureIgnoreCase))
        {
            Console.Clear();
            Visualizer.DrawGame(gameInstance);
            gameInstance.MoveGrid();
        } else if (input.Equals("O", StringComparison.CurrentCultureIgnoreCase))
        {
            GameOptionsMenu(gameInstance);
        } else if (input.Equals("M", StringComparison.CurrentCultureIgnoreCase))
        {
            bool moveSuccessful;
            do
            {
                moveSuccessful = gameInstance.MovePiece();
            } while (!moveSuccessful);
            Console.WriteLine("Type <x,y> - Insert new coordinates");
            Console.Write("> ");
            var newInput = Console.ReadLine()!;
            InsertCoordinates(gameInstance, newInput);
        }
    }
    
    private static void InsertCoordinates(TicTacTwoBrain gameInstance, string input)
    {
        var inputSplit = input.Split(",");

        if (inputSplit.Length != 2)
        {
            _invalidInput = true;
            return;
        }

        if (int.TryParse(inputSplit[0], out var inputX) && int.TryParse(inputSplit[1], out var inputY))
        {
            if (gameInstance.MakeAMoveCheck(inputX - 1, inputY - 1))
            {
                gameInstance.MakeAMove(inputX - 1, inputY - 1);
                AmountOfPiecesCounter(gameInstance);
            }
            else
            {
                _invalidMove = true;
            }
        }
    }
    
    private static void AmountOfPiecesCounter(TicTacTwoBrain gameInstance)
    {
        if (gameInstance.NextMoveBy == EGamePiece.O)
        {
            AmountOfPiecesX += 1;
        } else if (gameInstance.NextMoveBy == EGamePiece.X)
        {
            AmountOfPiecesO += 1;
        }
    }
    
    private static string InputCheck()
    {
        if (_invalidInput)
        {
            Console.WriteLine();
            return "\u001b[31mInvalid input!\u001b[0m";
        }
            
        if (_invalidMove)
        {
            Console.WriteLine();
            return "\u001b[31mYou can put piece only in the grid!\u001b[0m";
        }
        _invalidInput = false;
        _invalidMove = false;
        return "";
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
        var input = Console.ReadLine()!.ToUpper();

        switch (input)
        {
            case "B":
                break;
            case "R":
                gameInstance.ResetGame();
                break;
            case "S":
                Console.WriteLine("Insert game name: ");
                var gameName = Console.ReadLine() ?? "";
                GameRepository.SaveGame(gameInstance.GetGameStateJson(), gameInstance.GetGameConfigName(), gameName);
                Console.WriteLine("\u001b[32mGame Saved!\u001b[0m");
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
    
    public static string ChooseConfigurationNewGame()
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

        var chosenConfig = configMenu.Run();

        MainLoop(chosenConfig, "new");
        return "new";
    }
    
    public static string ChooseConfigurationLoadGame()
    {
        var configMenuItems = new List<MenuItem>();

        for (var i = 0; i < ConfigRepository.GetSavedGamesNames().Count; i++)
        {
            var titleString = ConfigRepository.GetSavedGamesNames()[i];
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

        MainLoop(chosenConfig, "load");
        return "load";
    }
}
