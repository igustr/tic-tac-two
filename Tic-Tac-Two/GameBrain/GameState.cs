namespace GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public EGamePiece[][] Grid { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;
    public GameConfiguration GameConfiguration { get; set; }
    public int _gridYMove = 0;
    public int _gridXMove = 0;
    public int _previousGridXMove;
    public int _previousGridYMove;
    
    public GameState(GameConfiguration gameConfiguration, EGamePiece[][] gameBoard, EGamePiece[][] grid)
    {
        GameBoard = gameBoard;
        Grid = grid;
        GameConfiguration = gameConfiguration;
    }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}