using DAL;
using GameBrain;
using ConsoleUI;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGameWebPlayer1 : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGameWebPlayer1(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public string? Error { get; set; }
    [BindProperty(SupportsGet = true)] public int ConfigId { get; set; } = default!;
    public string CurrentAction { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedX { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedY { get; set; } 
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }
    [BindProperty(SupportsGet = true)] public string GameMode { get; set; }
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public IActionResult OnGet(int? x, int? y, string? gameType)
    {
        OnGetLoadGame(gameType);
        
        if (TicTacTwoBrain.CheckWin())
        {
            if (TicTacTwoBrain.NextMoveBy == EGamePiece.O)
            {
                return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy});
            }
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }
        
        if (TicTacTwoBrain.NextMoveBy != EGamePiece.X)
        {
            TicTacTwoBrain.GridPlacement();
            Error = "It's not your turn!";
            return Page();
        }
        
        if (FinalStageCheck(TicTacTwoBrain) && gameType != "MovePiece")
        {
            CurrentAction = "SelectPiece";
            gameType = "SelectPiece";
        }
        
        // Load game state based on the game type
        if (gameType == "MovePiece" && x != null && y != null)
        {
            if (!TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value))
            {
                Error = "You can't place piece outside of grid!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
                return Page();
            }
            TicTacTwoBrain.MovePieceWeb(SelectedX, SelectedY, x.Value, y.Value);
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
        }
        else if (x != null && y != null && gameType != "MovePiece" && gameType != "SelectPiece")
        {
            if (!TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value))
            {
                Error = "You can't place piece outside of grid!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
                return Page();
            }
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
        } 

        TicTacTwoBrain.GridPlacement();


        if (TicTacTwoBrain.CheckWin())
        {
            if (TicTacTwoBrain.NextMoveBy == EGamePiece.O)
            {
                return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy});
            }
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }


        /*
        Console.WriteLine("grid X coords: " + string.Join(", ", TicTacTwoBrain.GridXCoordinates));
        Console.WriteLine("grid Y coords: " + string.Join(", ", TicTacTwoBrain.GridYCoordinates));
        */
        
        return Page();
    }

    public IActionResult OnPost(int? x, int? y, string action)
    {
        
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        var actionList = new List<string> { "U", "D", "L", "R", "UL", "UR", "DL", "DR" };

        if (action == "Refresh")
        {
            
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
            if (TicTacTwoBrain.CheckWin())
            {
                if (TicTacTwoBrain.NextMoveBy == EGamePiece.O)
                {
                    return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy});
                }
                return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
            }
            return Page();
        }
        
        CurrentAction = action;
        
        //Console.WriteLine("action POST: " + CurrentAction);
        
        if (actionList.Contains(CurrentAction))
        {

            TicTacTwoBrain.MoveGrid(CurrentAction);
            if (!TicTacTwoBrain.GridPlacement())
            {
                //Console.WriteLine("here in MoveGridCheckWeb");
                Error = "You can't move grid in this direction!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
                return Page();
            }
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
        } 
        else if (CurrentAction == "ChoosePiece" && x != null && y != null)
        {
            SelectedX = x.Value;
            SelectedY = y.Value;

            CurrentAction = "MovePiece";
        } 
        

        if (TicTacTwoBrain.CheckWin())
        {
            if (TicTacTwoBrain.NextMoveBy == EGamePiece.O)
            {
                return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy});
            }
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }


        return Page();
    }

    private void OnGetLoadGame(string? gameType)
    {
        if (gameType == "loadConfig")
        {
            var chosenConfig = _configRepository.GetConfigById(ConfigId);
            TicTacTwoBrain = new TicTacTwoBrain(chosenConfig);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, chosenConfig.Name);
        } 
        else if (gameType == "new")
        {
            TicTacTwoBrain = new TicTacTwoBrain(new GameConfiguration());
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "multiplayer");
        }
        else
        {
            var gameState = _gameRepository.LoadGame(GameId);
            TicTacTwoBrain = new TicTacTwoBrain(gameState);
        }
    }

    public bool FinalStageCheck(TicTacTwoBrain gameInstance)
    {
        if (gameInstance.NextMoveBy == EGamePiece.X && gameInstance.GetPiecesOnBoardCount().Item1 == gameInstance.AmountOfPieces)
        {
            return true;
        }
        if (gameInstance.NextMoveBy == EGamePiece.O && gameInstance.GetPiecesOnBoardCount().Item2 == gameInstance.AmountOfPieces)
        {
            return true;
        }
        return false;
    }

    private void FinalStageCheckAction(TicTacTwoBrain gameInstance)
    {
        if (FinalStageCheck(gameInstance))
        {
            CurrentAction = "SelectPiece";
        }
        else
        {
            CurrentAction = "SelectAction";
        }
    }
}
