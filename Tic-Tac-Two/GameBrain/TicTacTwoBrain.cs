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
        List<int> gridXCoordinatesList = [..GridXCoordinates];
        List<int> gridYCoordinatesList = [..GridYCoordinates];
        gridXCoordinatesList.Sort();
        gridYCoordinatesList.Sort();
        
        var board = GetBoard();
        var grid = new EGamePiece[GridXCoordinates.Count, GridYCoordinates.Count];
        for (var y = 0; y < GridYCoordinates.Count; y++)
        {
            for (var x = 0; x < GridXCoordinates.Count; x++)
            {
                /*
                Console.WriteLine(gridXCoordinatesList[x] + " " + gridYCoordinatesList[y] + " "
                                  + board[gridXCoordinatesList[x] - 1, gridYCoordinatesList[y] - 1] 
                                  + " y: " + y + " x: " + x);
                                  */
                grid[x, y] = board[gridXCoordinatesList[x] - 1, gridYCoordinatesList[y] - 1];
            }
        }
        return grid;
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
        
        /*
        Console.WriteLine("Grid start X: " + _gridX);
        Console.WriteLine("Grid start Y: " + _gridY);
        Console.WriteLine();
        */
        
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
        var board = GetBoard();
        Console.WriteLine("Here:" + board[0,0]);
        
        Console.WriteLine("-------------------------------");
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
                _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                break;
            case "D":
                Console.WriteLine("Moving grid Down.");
                _gridYMove += 1;
                _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                break;
            case "L":
                Console.WriteLine("Moving grid Left.");
                _gridXMove -= 1;
                _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                break;
            case "R":
                Console.WriteLine("Moving grid Right.");
                _gridXMove += 1;
                _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
                break;
            default:
                Console.WriteLine("Invalid direction. Please choose 'U', 'D', 'L', or 'R'.");
                break;
        }
    }

    private void MoveGridCheck()
    {
        if (GridXCoordinates.Max() > DimX || GridYCoordinates.Max() > DimY
                                          || GridXCoordinates.Min() <= 0 || GridYCoordinates.Min() <= 0)
        {
            Console.WriteLine("You can't make move in this direction. Choose different!");
            Console.WriteLine();

            _gridYMove = _previousGridYMove;
            _gridXMove = _previousGridXMove;
            
            MoveGrid();
            GridPlacement();
        }
    }

    public bool CheckWin()
    {
        var winCondition = _gameConfiguration.WinCondition;
        var gridSize = GridXCoordinates.Count;

        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j <= gridSize - winCondition; j++)
            {
                // horizontal
                if (j <= gridSize - winCondition && CheckLine(i, j, 0, 1))
                    return true;

                // vertical
                if (i <= gridSize - winCondition && CheckLine(i, j, 1, 0))
                    return true;

                // diagonal left to right
                if (i <= gridSize - winCondition && j <= gridSize - winCondition && CheckLine(i, j, 1, 1))
                    return true;

                // diagonal right to left
                if (i <= gridSize - winCondition && j >= winCondition - 1 && CheckLine(i, j, 1, -1))
                    return true;
            }
        }

        return false;
    }
    
    private bool CheckLine(int startX, int startY, int stepX, int stepY)
    {
        var grid = GetGrid();
        /*
        Console.WriteLine("HERE GRID AHAHAHAH 0,0: " + grid[0, 0]);
        Console.WriteLine("HERE GRID AHAHAHAH: 0,1: " + grid[0, 1]);
        Console.WriteLine("HERE GRID AHAHAHAH: 2,2: " + grid[2, 2]);
        */
        var winCondition = _gameConfiguration.WinCondition;
        var startSymbol = grid[startX, startY];
        
        if (startSymbol == EGamePiece.Empty) return false;

        for (var i = 1; i < winCondition; i++)
        {
            if (grid[startX + i * stepX, startY + i * stepY] != startSymbol)
                return false;
        }

        return true;
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




