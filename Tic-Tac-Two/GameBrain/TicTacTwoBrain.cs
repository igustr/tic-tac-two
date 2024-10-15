namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    private EGamePiece[,] _grid;
    private EGamePiece _nextMoveBy { get; set; } = EGamePiece.X;

    private GameConfiguration _gameConfiguration;
    
    private static int _gridX;
    private static int _gridY;
    private static int _gridYMove = 0;
    private static int _gridXMove = 0;
    private static int _previousGridXMove;
    private static int _previousGridYMove;
    private static readonly HashSet<int> GridXCoordinates = [];
    private static readonly HashSet<int> GridYCoordinates = [];
    
    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gameBoard = new EGamePiece[_gameConfiguration.BoardSizeWidth, _gameConfiguration.BoardSizeHeight];
        _grid = new EGamePiece[_gameConfiguration.GridSizeWidth, _gameConfiguration.GridSizeHeight];
    }
    
    public EGamePiece[,] GameBoard
    {
        get => GetBoard();
        private set => _gameBoard = value;
    }
    
    public EGamePiece[,] Grid
    {
        get => GetGrid();
        private set => _grid = value;
    }

    public int DimX => _gameBoard.GetLength(0);
    public int DimY => _gameBoard.GetLength(1);
    public int GridX => _grid.GetLength(0);
    public int GridY => _grid.GetLength(1);
    
    private EGamePiece[,] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var x = 0; x < _gameBoard.GetLength(0); x++)
        {
            for (var y = 0; y < _gameBoard.GetLength(1); y++)
            {
                copyOfBoard[x, y] = _gameBoard[x, y];
            }
        }

        return copyOfBoard;
    }

    private EGamePiece[,] GetGrid()
    {
        var copyOfGrid = new EGamePiece[_grid.GetLength(0), _grid.GetLength(1)];
        for (var x = 0; x < _grid.GetLength(0); x++)
        {
            for (var y = 0; y < _grid.GetLength(1); y++)
            {
                copyOfGrid[x, y] = _grid[x, y];
            }
        }
        
        return copyOfGrid;
    }
    
    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x, y] = _nextMoveBy;
        
        // flip the next piece
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        return true;
    }
    
    public void GridPlacement()
    {
        GridXCoordinates.Clear();
        GridYCoordinates.Clear();
        
        _gridX = (DimX % 2 == 0) ? DimX / 2 : DimX / 2 + 1;
        _gridY = (DimY % 2 == 0) ? DimY / 2 : DimY / 2 + 1;
        
        _gridX += _gridXMove;
        _gridY += _gridYMove;
        
        GridYCoordinates.Add(_gridY);
        GridXCoordinates.Add(_gridX);
        
        Console.WriteLine("Grid start X: " + _gridX);
        Console.WriteLine("Grid start Y: " + _gridY);
        Console.WriteLine();
        
        for (int i = 0; i < GridY - 2; i++)
        {
            GridYCoordinates.Add(_gridY + (i + 1));
            GridYCoordinates.Add(_gridY + (i - 1));
        }
        for (int i = 0; i < GridX - 2; i++) {
            GridXCoordinates.Add(_gridX + (i + 1));
            GridXCoordinates.Add(_gridX + (i - 1));
        }
        
        Console.WriteLine("grid x coords: " + string.Join(", ", GridXCoordinates));
        Console.WriteLine("grid y coords: " + string.Join(", ", GridYCoordinates));
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine();

        MoveGridCheck();
    }

    public void MoveGrid()
    {        
        Console.WriteLine("Choose grid movement direction:");
        Console.WriteLine("Use one of the following directions: ");
        Console.WriteLine("'U' for Up, 'D' for Down, 'L' for Left, 'R' for Right.");
        Console.Write("> ");
        var directionInput = Console.ReadLine()!.ToUpper();

        _previousGridYMove = _gridYMove;
        _previousGridXMove = _gridXMove;
        
        switch (directionInput)
        {
            case "U":
                Console.WriteLine("Moving grid Up.");
                _gridYMove -= 1;
                break;
            case "D":
                Console.WriteLine("Moving grid Down.");
                _gridYMove += 1;
                break;
            case "L":
                Console.WriteLine("Moving grid Left.");
                _gridXMove -= 1;
                break;
            case "R":
                Console.WriteLine("Moving grid Right.");
                _gridXMove += 1;
                break;
            default:
                Console.WriteLine("Invalid direction. Please choose 'U', 'D', 'L', or 'R'.");
                break;
        }
    }

    private void MoveGridCheck()
    {
        Console.WriteLine("1) Grid Y move: " + _gridYMove + " Grid X Move: " + _gridXMove);
        if (GridXCoordinates.Max() > DimX || GridYCoordinates.Max() > DimY
                                          || GridXCoordinates.Min() <= 0 || GridYCoordinates.Min() <= 0)
        {
            Console.WriteLine("You can't make move in this direction. Choose different!");
            Console.WriteLine();

            _gridYMove = _previousGridYMove;
            _gridXMove = _previousGridXMove;
            
            Console.WriteLine("2) Grid Y move: " + _gridYMove + " Grid X Move: " + _gridXMove);
            
            MoveGrid();
            GridPlacement();
        }
    }

    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        _nextMoveBy = EGamePiece.X;
    }

    public static HashSet<int> GetGridXCoordinates()
    {
        return GridXCoordinates;
    }
    
    public static HashSet<int> GetGridYCoordinates()
    {
        return GridYCoordinates;
    }
}




