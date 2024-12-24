using System.Globalization;
using ConsoleUI;
using DAL;
using Domain;
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
                if (gameInstance.NextMoveBy == EGamePiece.X)
                {
                    Console.WriteLine("Winner is: " + EGamePiece.O);
                }
                else
                { 
                    Console.WriteLine("Winner is: " + EGamePiece.X);
                }
                GameEndScreen();
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
                var amountOfPiecesXOnBoard = gameInstance.GetPiecesOnBoardCount().Item1;
                
                if (amountOfPiecesXOnBoard < _movePieceAfterNMoves)
                {
                    GamePhase.InitialPlacementPhase(gameInstance, gameRepository);
                }
                else if (amountOfPiecesXOnBoard == chosenConfig.AmountOfPieces)
                {
                    GamePhase.FinalMovementPhase(gameInstance, gameRepository);
                }
                else if (amountOfPiecesXOnBoard >= _movePieceAfterNMoves)
                {
                    GamePhase.PlacementAndMovementPhase(gameInstance, gameRepository);
                }
            } 
            
            // Player O Turn
            else if (gameInstance.NextMoveBy == EGamePiece.O)
            {
                var amountOfPiecesOOnBoard = gameInstance.GetPiecesOnBoardCount().Item2;
                
                if (amountOfPiecesOOnBoard < _movePieceAfterNMoves)
                {
                    GamePhase.InitialPlacementPhase(gameInstance, gameRepository);
                }
                else if (amountOfPiecesOOnBoard == chosenConfig.AmountOfPieces)
                {
                    GamePhase.FinalMovementPhase(gameInstance, gameRepository);
                }
                else if (amountOfPiecesOOnBoard >= _movePieceAfterNMoves)
                {
                    GamePhase.PlacementAndMovementPhase(gameInstance, gameRepository);
                }
            }
        } while (true);
    }
    
    public static void InsertCoordinates(TicTacTwoBrain gameInstance, string input)
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

    private static void GameEndScreen()
    {
        Console.WriteLine();
        Console.WriteLine("B) Back in Menu");
        Console.WriteLine("E) Exit");

        Console.Write("> ");
        var input = Console.ReadLine()!;

        if (input.Equals("B", StringComparison.CurrentCultureIgnoreCase))
        {
            Menus.MainMenu.Run();
        }
        else
        {
            Environment.Exit(0);
        }
    }
}
