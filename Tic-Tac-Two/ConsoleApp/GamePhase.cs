using ConsoleUI;
using DAL;
using GameBrain;

namespace ConsoleApp;

public class GamePhase
{
    public static void InitialPlacementPhase(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
            GameController.InsertCoordinates(gameInstance, input);
        }
    }
    
    public static void PlacementAndMovementPhase(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
            GameController.InsertCoordinates(gameInstance, newInput);
        }
        else
        {
            GameController.InsertCoordinates(gameInstance, input);
        }
    }

    public static void FinalMovementPhase(TicTacTwoBrain gameInstance, IGameRepository gameRepository)
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
            GameController.InsertCoordinates(gameInstance, newInput);
        }
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