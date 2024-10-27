namespace GameBrain;

public class GameState
{
    public EGamePiece[,] GameBoard;
    public EGamePiece[,] Grid;
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;
    public GameConfiguration GameConfiguration;
    public int _gridX;
    public int _gridY;
    public int _gridYMove = 0;
    public int _gridXMove = 0;
    public int _previousGridXMove;
    public int _previousGridYMove;
    
    public GameState(GameConfiguration gameConfiguration, EGamePiece[,] gameBoard, EGamePiece[,] grid)
    {
        GameBoard = gameBoard;
        Grid = grid;
        GameConfiguration = gameConfiguration;
    }
}