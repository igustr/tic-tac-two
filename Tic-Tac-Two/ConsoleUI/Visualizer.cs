using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    private static HashSet<int> _gridXCoordinates = [];
    private static HashSet<int> _gridYCoordinates = [];
    
    public static void DrawGame(TicTacTwoBrain gameInstance)
    {
        //Console.Clear();
        Console.WriteLine("==================================");
        Console.WriteLine("Player: " + gameInstance.NextMoveBy);
        Console.WriteLine();
        
        gameInstance.GridPlacement();
        _gridXCoordinates = TicTacTwoBrain.GetGridXCoordinates();
        _gridYCoordinates = TicTacTwoBrain.GetGridYCoordinates();
        
        for (var x = 0; x < gameInstance.DimX; x++)
        {
            if (x < 10)
            {
                Console.Write("  " + (x + 1) + " ");
            }
            else
            {
                Console.Write(" " + (x + 1) + " ");
            }
        }
        Console.WriteLine();
        
        for (var y = 0; y < gameInstance.DimY; y++)
        {
            Console.Write(y + 1);
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x][y]) + " ");
                if (x == gameInstance.DimX - 1) continue;
                if (_gridXCoordinates.Contains(x + 1) 
                    && _gridYCoordinates.Contains(y + 1)
                    && x != _gridXCoordinates.Max() - 1)
                {
                    Console.Write("\u001b[31m|\u001b[0m");
                }
                else
                {
                    Console.Write("|");
                }
            }

            Console.WriteLine();
            Console.Write(" ");
            if (y == gameInstance.DimY - 1) continue;
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if (_gridXCoordinates.Contains(x + 1) 
                    && _gridYCoordinates.Contains(y + 1) 
                    && y != _gridYCoordinates.Max() - 1)
                {
                    Console.Write("\u001b[31m---\u001b[0m");
                    if (x != _gridXCoordinates.Max() - 1)
                    {
                        Console.Write("\u001b[31m+\u001b[0m");
                    } else 
                    {
                        Console.Write("+");
                    }
                }
                else
                {
                    Console.Write("---");
                    if (x != gameInstance.DimX - 1)
                    {
                        Console.Write("+");
                    } 
                }
            }

            Console.WriteLine();
        }
    }

    public static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}





