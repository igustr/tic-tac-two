namespace GameBrain;

public class TicTacTwoBrain
{
    
    public static HashSet<int> GridXCoordinates = new();
    public static HashSet<int> GridYCoordinates = new();
    private readonly GameState _gameState;
    private int _gridXCentre;
    private int _gridYCentre;
    private int _previousGridXShift;
    private int _previousGridYShift;
    
    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        var gameBoard = new EGamePiece[gameConfiguration.BoardSizeWidth][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[gameConfiguration.BoardSizeHeight];
        }
        
        var grid = new EGamePiece[gameConfiguration.GridSizeWidth][];
        for (var x = 0; x < grid.Length; x++)
        {
            grid[x] = new EGamePiece[gameConfiguration.GridSizeHeight];
        }

        _gameState = new GameState(
            gameConfiguration,
            gameBoard,
            grid
        );
    }

    public TicTacTwoBrain(GameState gameState)
    {
        _gameState = gameState;
    }

    public string GetGameStateJson() => _gameState.ToString();
    public EGamePiece[][] GameBoard => GetBoard();
    public int DimX => _gameState.GameBoard.Length;
    public int DimY => _gameState.GameBoard[0].Length;
    public int GridX => _gameState.Grid.Length;
    public int GridY => _gameState.Grid[0].Length;
    public EGamePiece NextMoveBy => _gameState.NextMoveBy;
    public int AmountOfPieces => _gameState.GameConfiguration.AmountOfPieces;
    public int MovePieceAfterNMoves => _gameState.GameConfiguration.MovePieceAfterNMoves;
    public HashSet<int> gridXCoordinates = GetGridXCoordinates();
    public HashSet<int> gridYCoordinates = GetGridYCoordinates();
    
    private EGamePiece[][] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameState.GameBoard.GetLength(0)][];
            //, _gameState.GameBoard.GetLength(1)];
        for (var x = 0; x < _gameState.GameBoard.Length; x++)
        {
            copyOfBoard[x] = new EGamePiece[_gameState.GameBoard[x].Length];
            for (var y = 0; y < _gameState.GameBoard[x].Length; y++)
            {
                copyOfBoard[x][y] = _gameState.GameBoard[x][y];
            }
        }
        return copyOfBoard;
    }
    
    private EGamePiece[][] GetGrid()
    {
        List<int> gridXCoordinatesList = GridXCoordinates.ToList();
        List<int> gridYCoordinatesList = GridYCoordinates.ToList();
        gridXCoordinatesList.Sort();
        gridYCoordinatesList.Sort();
        
        var board = GetBoard();
        var grid = new EGamePiece[GridYCoordinates.Count][];
    
        for (var y = 0; y < GridYCoordinates.Count; y++)
        {
            grid[y] = new EGamePiece[GridXCoordinates.Count];
            for (var x = 0; x < GridXCoordinates.Count; x++)
            {
                grid[y][x] = board[gridXCoordinatesList[x] - 1][gridYCoordinatesList[y] - 1];
            }
        }
        return grid;
    }
    
    public void MakeAMove(int x, int y)
    {
        _gameState.GameBoard[x][y] = _gameState.NextMoveBy;
        
            // flip the next piece
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

    }

    public bool MakeAMoveCheck(int x, int y)
    {

        /*
        Console.WriteLine("X COORDS: " + x);
        Console.WriteLine("Y COORDS: " + y);
        Console.WriteLine("position key: " + _gameState.GameBoard[x][y]);
        */
        
        if (_gameState.GameBoard[x][y] != EGamePiece.Empty 
            || !GridXCoordinates.Contains(x + 1) 
            || !GridYCoordinates.Contains(y + 1))
        {
            return false;
        }

        return true;
    }

    public bool MovePiece()
    {
        Console.WriteLine("Type <x,y> - Insert Piece coordinates");
        Console.Write("> ");
        var input = Console.ReadLine()!;
        var inputSplit = input.Split(",");

        if (inputSplit.Length != 2 || 
            !int.TryParse(inputSplit[0], out var x) || 
            !int.TryParse(inputSplit[1], out var y))
        {
            Console.WriteLine("\u001b[31mInvalid coordinates format. Please use <x,y> format.\u001b[0m");
            return false;
        }
        
        Console.WriteLine("GameBoard: " + _gameState.GameBoard[x - 1][y - 1]);
       // Console.WriteLine(x + " " +  y);
        
        // Validate that the chosen piece belongs to the current player and is not empty
        if (_gameState.GameBoard[x - 1][y - 1] == _gameState.NextMoveBy)
        {
            // Mark the original location as empty
            _gameState.GameBoard[x - 1][y - 1] = EGamePiece.Empty;
            Console.WriteLine("GameBoard 2: " + _gameState.GameBoard[x - 1][y - 1]);
            return true;
        }
        else
        {
            Console.WriteLine("\u001b[31mInvalid piece selection. Choose a piece belonging to you.\u001b[0m");
            return false;
        }
    }
    
    public void GridPlacement()
    {
        GridXCoordinates.Clear();
        GridYCoordinates.Clear();
        
        _gridXCentre = (DimX % 2 == 0) ? DimX / 2 : DimX / 2 + 1;
        _gridYCentre = (DimY % 2 == 0) ? DimY / 2 : DimY / 2 + 1;
        
        _gridXCentre += _gameState.GridXPosition;
        _gridYCentre += _gameState.GridYPosition;
        
        GridYCoordinates.Add(_gridYCentre);
        GridXCoordinates.Add(_gridXCentre);
        
        var rangeX = (int)Math.Ceiling((GridX - 1) / 2.0); // Determines how many cells to add left and right
        var rangeY = (int)Math.Ceiling((GridY - 1) / 2.0); // Determines how many cells to add up and down

        Console.WriteLine("Grid shift X: " + _gameState.GridXPosition);
        Console.WriteLine("Grid shift Y: " + _gameState.GridYPosition);

        /*
        Console.WriteLine("range X: " + rangeX);
        Console.WriteLine("range Y: " + rangeY);
        Console.WriteLine();
        Console.WriteLine("Grid start X: " + _gridXCentre);
        Console.WriteLine("Grid start Y: " + _gridYCentre);
        Console.WriteLine();
        */
        
        for (var i = 0; i < rangeY; i++)
        {
            if (GridY % 2 == 0)
            {
                GridYCoordinates.Add(_gridYCentre + (i + 1));
                if (rangeY != i + 1)
                {
                    GridYCoordinates.Add(_gridYCentre - (i + 1));
                }
                
            }
            else
            {
                GridYCoordinates.Add(_gridYCentre + (i + 1));
                GridYCoordinates.Add(_gridYCentre - (i + 1));
            }
        }
        for (var i = 0; i < rangeX; i++) {
            if (GridX % 2 == 0)
            {
                GridXCoordinates.Add(_gridXCentre + (i + 1));
                if (rangeX != i + 1)
                {
                    GridXCoordinates.Add(_gridXCentre - (i + 1));
                }
            } else
            {
                GridXCoordinates.Add(_gridXCentre + (i + 1));
                GridXCoordinates.Add(_gridXCentre - (i + 1));
            }
        }
        
        /*
        Console.WriteLine("grid x coords: " + string.Join(", ", GridXCoordinates));
        Console.WriteLine("grid y coords: " + string.Join(", ", GridYCoordinates));
        Console.WriteLine();
        */

        MoveGridCheck();
    }

    public void MoveGrid(string directionInput)
    {
        _previousGridYShift = _gameState.GridYPosition;
        _previousGridXShift = _gameState.GridXPosition;

        switch (directionInput)
        {
            case "U":
                Console.WriteLine("Moving grid Up.");
                _gameState.GridYPosition -= 1;
                break;
            case "D":
                Console.WriteLine("Moving grid Down.");
                _gameState.GridYPosition += 1;
                break;
            case "L":
                Console.WriteLine("Moving grid Left.");
                _gameState.GridXPosition -= 1;
                break;
            case "R":
                Console.WriteLine("Moving grid Right.");
                _gameState.GridXPosition += 1;
                break;
            case "UL":
                Console.WriteLine("Moving grid Up-Left.");
                _gameState.GridYPosition -= 1;
                _gameState.GridXPosition -= 1;
                break;
            case "UR":
                Console.WriteLine("Moving grid Up-Right.");
                _gameState.GridYPosition -= 1;
                _gameState.GridXPosition += 1;
                break;
            case "DL":
                Console.WriteLine("Moving grid Down-Left.");
                _gameState.GridYPosition += 1;
                _gameState.GridXPosition -= 1;
                break;
            case "DR":
                Console.WriteLine("Moving grid Down-Right.");
                _gameState.GridYPosition += 1;
                _gameState.GridXPosition += 1;
                break;
            case "B":
                Console.WriteLine("Going Back");
                break;
            default:
                Console.WriteLine("\u001b[31mYou can't make move in this direction. Choose different!\u001b[0m");
                break;
        }

        // Toggle the next player after each move, if a valid move was made
        if (directionInput != "B" && "UDLRULURDLDR".Contains(directionInput))
        {
            _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        }
    }

    private void MoveGridCheck()
    {
        if (GridXCoordinates.Max() > DimX || GridYCoordinates.Max() > DimY
                                                     || GridXCoordinates.Min() <= 0 
                                                     || GridYCoordinates.Min() <= 0)
        {
            _gameState.GridYPosition = _previousGridYShift;
            _gameState.GridXPosition = _previousGridXShift;
            
            MoveGrid("null");
            GridPlacement();
        }
    }

    public bool CheckWin()
    {
        var winCondition = _gameState.GameConfiguration.WinCondition;
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
        var winCondition = _gameState.GameConfiguration.WinCondition;
        var startSymbol = grid[startX][startY];
        
        if (startSymbol == EGamePiece.Empty) return false;

        for (var i = 1; i < winCondition; i++)
        {
            if (grid[startX + i * stepX][startY + i * stepY] != startSymbol)
                return false;
        }

        return true;
    }

    public void ResetGame()
    {
        var gameBoard = new EGamePiece[_gameState.GameConfiguration.BoardSizeWidth][];
        for (var x = 0; x < gameBoard.Length; x++)
        {
            gameBoard[x] = new EGamePiece[_gameState.GameConfiguration.BoardSizeHeight];
        }
        
        var grid = new EGamePiece[_gameState.GameConfiguration.GridSizeWidth][];
        for (var x = 0; x < grid.Length; x++)
        {
            grid[x] = new EGamePiece[_gameState.GameConfiguration.GridSizeHeight];
        }

        _gameState.GameBoard = gameBoard;
        _gameState.Grid = grid;
        _gameState.NextMoveBy = EGamePiece.X;
    }

    public void ExitGame()
    {
        //IMPLEMENT
    }

    public static HashSet<int> GetGridXCoordinates()
    {
        return GridXCoordinates;
    }
    
    public static HashSet<int> GetGridYCoordinates()
    {
        return GridYCoordinates;
    }
    
    public (int xCount, int oCount) GetPiecesOnBoardCount()
    {
        var xCount = 0;
        var oCount = 0;

        foreach (var row in _gameState.GameBoard)
        {
            foreach (var piece in row)
            {
                if (piece == EGamePiece.X)
                    xCount++;
                else if (piece == EGamePiece.O)
                    oCount++;
            }
        }

        return (xCount, oCount);
    }
    
}
