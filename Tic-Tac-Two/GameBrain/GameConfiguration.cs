namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;

    // how many pieces in straight to win the game
    public int WinCondition { get; set; } = 3;

    // 0 disabled
    public int MovePieceAfterNMoves { get; set; } = 2;
    public int AmountOfPieces { get; set; } = 4;
    public int GridSizeHeight { get; set; } = 3;
    public int GridSizeWidth { get; set; } = 3;


    
    //TODO: Check if number in user input and put some limits on size. Check negative amount.
    public string ToJsonString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win: {WinCondition}, " +
        $"can move piece after {MovePieceAfterNMoves} moves";
}