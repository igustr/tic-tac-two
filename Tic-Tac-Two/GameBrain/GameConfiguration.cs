namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;

    // how many pieces in straight to win the game
    public int WinCondition { get; set; } = 3;

    // 0 disabled
    public int MovePieceAfterNMoves { get; set; } = 5;
    public int AmountOfPieces { get; set; } = 4;
    public int GridSizeHeight { get; set; } = 3;
    public int GridSizeWidth { get; set; } = 3;
    
    private int _size;
    private int _gridSize;

    
    //TODO: Check if number in user input and put some limits on size. Check negative amount.
    public void CustomGameCheck()
    {
        if (Name == "Custom")
        {
            // Min board size 5x5
            bool isValidBoardSize = false;
            while (!isValidBoardSize)
            {
                Console.WriteLine("Board size: ");
                _size = int.Parse(Console.ReadLine());

                if (_size >= 5)
                {
                    BoardSizeHeight = _size;
                    BoardSizeWidth = _size;
                    isValidBoardSize = true;
                }
                else
                {
                    Console.WriteLine("Invalid board size! The board size must be at least 5.");
                }
            }

            // Min grid size 3x3
            bool isValidGridSize = false;
            while (!isValidGridSize)
            {
                Console.WriteLine("Grid Size: ");
                _gridSize = int.Parse(Console.ReadLine());

                if (_gridSize >= 3)
                {
                    GridSizeHeight = _gridSize;
                    GridSizeWidth = _gridSize;
                    isValidGridSize = true;
                }
                else
                {
                    Console.WriteLine("Invalid grid size! The grid size must be at least 3.");
                }
            }

            // Min amount of pieces 3
            bool isValidAmountOfPieces = false;
            while (!isValidAmountOfPieces)
            {
                Console.WriteLine("Amount of Pieces: ");
                AmountOfPieces = int.Parse(Console.ReadLine());

                if (AmountOfPieces >= 3)
                {
                    MovePieceAfterNMoves = AmountOfPieces / 2;
                    isValidAmountOfPieces = true;
                }
                else
                {
                    Console.WriteLine("Invalid amount of pieces! The amount must be at least 3.");
                }
            }

            // Must be less than or equal to Amount of Pieces
            bool isValidWinCondition = false;
            while (!isValidWinCondition)
            {
                Console.WriteLine("Amount of Pieces to Win: ");
                WinCondition = int.Parse(Console.ReadLine());

                if (WinCondition <= AmountOfPieces && WinCondition > 0)
                {
                    isValidWinCondition = true;
                }
                else
                {
                    Console.WriteLine(
                        "Invalid input! Amount of Pieces to Win should be less than or equal to the Amount of Pieces and greater than 0.");
                }
            }
        }
    }


    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win: {WinCondition}, " +
        $"can move piece after {MovePieceAfterNMoves} moves";
}