namespace GameBrain;

public class TicTacTwoBrain
{
    
    public static HashSet<int> GridXCoordinates = new();
    public static HashSet<int> GridYCoordinates = new();
    
    private readonly GameState _gameState;
    private int _gridX;
    private int _gridY;
    
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
    
    public string GetGameStateJson() => _gameState.ToString();
    public string GetGameConfigName() => _gameState.GameConfiguration.Name;
    public EGamePiece[][] GameBoard => GetBoard();
    public int DimX => _gameState.GameBoard.Length;
    public int DimY => _gameState.GameBoard[0].Length;
    public int GridX => _gameState.Grid.Length;
    public int GridY => _gameState.Grid[0].Length;
    public EGamePiece NextMoveBy => _gameState.NextMoveBy;
    
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
        Console.WriteLine("X COORDS: " + string.Join(", ", GridXCoordinates));
        Console.WriteLine("Y COORDS: " + string.Join(", ", GridYCoordinates));
        */
        if (_gameState.GameBoard[x][y] != EGamePiece.Empty 
            || !GridXCoordinates.Contains(y + 1) 
            || !GridYCoordinates.Contains(x + 1))
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
        
        _gridX = (DimX % 2 == 0) ? DimX / 2 : DimX / 2 + 1;
        _gridY = (DimY % 2 == 0) ? DimY / 2 : DimY / 2 + 1;
        
        _gridX += _gameState._gridXMove;
        _gridY += _gameState._gridYMove;
        
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
        
        /*
        Console.WriteLine("grid x coords: " + string.Join(", ", GridXCoordinates));
        Console.WriteLine("grid y coords: " + string.Join(", ", GridYCoordinates));
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine();
        */


        MoveGridCheck();
    }

    public void MoveGrid()
    {
        var board = GetBoard();
        Console.WriteLine();
        Console.WriteLine("Choose grid movement direction");
        Console.WriteLine("-------------------------------");
        Console.WriteLine("Use one of the following directions:");
        Console.WriteLine(
            "'U' for Up, 'D' for Down, 'L' for Left, 'R' for Right, 'UL' for Up-Left, 'UR' for Up-Right, 'DL' for Down-Left, 'DR' for Down-Right, 'B' to go back!");
        Console.Write("> ");
        var directionInput = Console.ReadLine()!.ToUpper();

        _gameState._previousGridYMove = _gameState._gridYMove;
        _gameState._previousGridXMove = _gameState._gridXMove;

        switch (directionInput)
        {
            case "U":
                Console.WriteLine("Moving grid Up.");
                _gameState._gridYMove -= 1;
                break;
            case "D":
                Console.WriteLine("Moving grid Down.");
                _gameState._gridYMove += 1;
                break;
            case "L":
                Console.WriteLine("Moving grid Left.");
                _gameState._gridXMove -= 1;
                break;
            case "R":
                Console.WriteLine("Moving grid Right.");
                _gameState._gridXMove += 1;
                break;
            case "UL":
                Console.WriteLine("Moving grid Up-Left.");
                _gameState._gridYMove -= 1;
                _gameState._gridXMove -= 1;
                break;
            case "UR":
                Console.WriteLine("Moving grid Up-Right.");
                _gameState._gridYMove -= 1;
                _gameState._gridXMove += 1;
                break;
            case "DL":
                Console.WriteLine("Moving grid Down-Left.");
                _gameState._gridYMove += 1;
                _gameState._gridXMove -= 1;
                break;
            case "DR":
                Console.WriteLine("Moving grid Down-Right.");
                _gameState._gridYMove += 1;
                _gameState._gridXMove += 1;
                break;
            case "B":
                Console.WriteLine("Going Back");
                break;
            default:
                Console.WriteLine("Invalid direction. Please choose 'U', 'D', 'L', 'R', 'UL', 'UR', 'DL', or 'DR'.");
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
            Console.WriteLine("You can't make move in this direction. Choose different!");
            Console.WriteLine();

            _gameState._gridYMove = _gameState._previousGridYMove;
            _gameState._gridXMove = _gameState._previousGridXMove;
            
            MoveGrid();
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
}
