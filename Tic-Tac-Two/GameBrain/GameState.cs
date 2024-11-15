namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece[][] Grid { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;
    public GameConfiguration GameConfiguration { get; set; }
    public int GridYPosition { get; set; }
    public int GridXPosition { get; set; }
    
    public GameState(GameConfiguration gameConfiguration, EGamePiece[][] gameBoard, EGamePiece[][] grid)
    {
        GameBoard = gameBoard;
        Grid = grid;
        GameConfiguration = gameConfiguration;
        GridYPosition = 0;
        GridXPosition = 0;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}