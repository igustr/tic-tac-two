using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    /*
    private static int _gridX;
    private static int _gridY;
    public static int GridYMove = 0;
    public static int GridXMove = 0;
    */
    private static HashSet<int> GridXCoordinates = [];
    private static HashSet<int> GridYCoordinates = [];

    
    //TODO for grid i need to hold somewhere position and draw it out by position
    //Mb detect grid by left upper corner and draw out x + grid.lenght ????
    
    public static void DrawGame(TicTacTwoBrain gameInstance)
    {
        gameInstance.GridPlacement();
        GridXCoordinates = TicTacTwoBrain.GetGridXCoordinates();
        GridYCoordinates = TicTacTwoBrain.GetGridYCoordinates();
        
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
                if (GridXCoordinates.Contains(x + 1) 
                    && GridYCoordinates.Contains(y + 1)
                    && x != GridXCoordinates.Max() - 1)
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
                if (GridXCoordinates.Contains(x + 1) 
                    && GridYCoordinates.Contains(y + 1) 
                    && y != GridYCoordinates.Max() - 1)
                {
                    Console.Write("\u001b[31m---\u001b[0m");
                    if (x != GridXCoordinates.Max() - 1)
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
                
                
                // TODO: Nothing should be hardcoded, size of the board can be also 10x10. Min size 5x5.
                // TODO: Pieces amount should also variey on size of board.  
                // TODO: support any size of board, grid, and number of pieces.
        }
    }

    private static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}





