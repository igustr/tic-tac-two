namespace GameBrain;

public record struct GameConfiguration()
{
   
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;

    // how many pieces in straight to win the game
    public int WinCondition { get; set; } = 3;

    // 0 disabled
    public int MovePieceAfterNMoves { get; set; } = 0;
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
            Console.WriteLine("Board size: ");
            _size = int.Parse(Console.ReadLine());
            BoardSizeHeight = _size; 
            BoardSizeWidth = _size;
            
            Console.WriteLine("Amount of Pieces: ");
            AmountOfPieces = int.Parse(Console.ReadLine());
            
            Console.WriteLine("Grid Size: ");
            _gridSize = int.Parse(Console.ReadLine());
            GridSizeHeight = _gridSize;
            GridSizeWidth = _gridSize;
            
            Console.WriteLine("Amount of Pieces to Win (Should be more or equal to amount of pieces): ");
            WinCondition = int.Parse(Console.ReadLine());
        }
    }

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win: {WinCondition}, " +
        $"can move piece after {MovePieceAfterNMoves} moves";
}