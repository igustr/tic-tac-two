using System.Globalization;
using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{

    private static bool _invalidInput;
    private static bool _invalidMove;
    private static int _movePieceAfterNMoves = 2;
    
    public static string MainLoop(string chosenConfigShortcut, string gameType,
        IConfigRepository configRepository, IGameRepository gameRepository)
    {
        if (!int.TryParse(chosenConfigShortcut, out var configNo))
        {
            return chosenConfigShortcut;
        }

        GameConfiguration chosenConfig;
        TicTacTwoBrain gameInstance;
        
        if (gameType == "load")
        {
            Console.WriteLine("configNo: " + configNo);
            var gameState = gameRepository.GetSavedGameByName(
                gameRepository.GetSavedGamesNames()[configNo]
            );
            chosenConfig = gameState.GameConfiguration;
    
            gameInstance = new TicTacTwoBrain(gameState);
        } 
        else 
        {
            // load config and start new game
            if (gameType == "loadConfig")
            {
                var allConfigs = configRepository.GetConfigurationNames();
                chosenConfig = configRepository.GetConfigurationByName(allConfigs[configNo]);
                
            } 
            // custom game
            else if (chosenConfigShortcut == "2")
            {
                chosenConfig = ConfigsController.CustomGameCheck();
                _movePieceAfterNMoves = chosenConfig.MovePieceAfterNMoves;
                Console.WriteLine("Do you want to save this configuration?(Y/N): ");
                Console.Write("> ");
                var input = Console.ReadLine()!;
                if (input.ToUpper().Equals("Y"))
                { 
                    chosenConfig.Name = "User Custom"; 
                    configRepository.SaveConfig(chosenConfig.ToJsonString());
                }
            }
            // new game
            else
            {
                chosenConfig = new GameConfiguration();
            }
            gameInstance = new TicTacTwoBrain(chosenConfig);
        }
        
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
                var amountOfPiecesXOnBoard = gameInstance.GetPieceCounts().Item1;
                
                if (amountOfPiecesXOnBoard < _movePieceAfterNMoves)
                {
                    FirstLevel(gameInstance, gameRepository);
                }
                else if (amountOfPiecesXOnBoard == chosenConfig.AmountOfPieces)
                {
                    ThirdLevel(gameInstance, gameRepository);
                }
                else if (amountOfPiecesXOnBoard >= _movePieceAfterNMoves)
                {
                    SecondLevel(gameInstance, gameRepository);
                }
            } 
            
            // Player O Turn
            else if (gameInstance.NextMoveBy == EGamePiece.O)
            {
                var amountOfPiecesOOnBoard = gameInstance.GetPieceCounts().Item2;
                
                if (amountOfPiecesOOnBoard < _movePieceAfterNMoves)
                {
                    FirstLevel(gameInstance, gameRepository);
                }
                else if (amountOfPiecesOOnBoard == chosenConfig.AmountOfPieces)
                {
                    ThirdLevel(gameInstance, gameRepository);
                }
                else if (amountOfPiecesOOnBoard >= _movePieceAfterNMoves)
                {
                    SecondLevel(gameInstance, gameRepository);
                }
            }
        } while (true);
    }
    
    private static void FirstLevel(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
    {
        Console.WriteLine();
        Console.WriteLine("1) Type <x,y> - Insert coordinates");
        Console.WriteLine("O) Options: ");
        Console.Write("> ");
        var input = Console.ReadLine()!;

        if (input.Equals("O", StringComparison.CurrentCultureIgnoreCase))
        {
            GameOptionsMenu(gameInstance, gameRepository);
        }
        else
        {
            InsertCoordinates(gameInstance, input);
        }
    }

    private static void SecondLevel(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
            GameOptionsMenu(gameInstance, gameRepository);
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

    private static void ThirdLevel(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
            GameOptionsMenu(gameInstance, gameRepository);
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
            }
            else
            {
                _invalidMove = true;
            }
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

    private static void GameOptionsMenu(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
                gameRepository.SaveGame(gameInstance.GetGameStateJson(), -1, gameName);
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
}
