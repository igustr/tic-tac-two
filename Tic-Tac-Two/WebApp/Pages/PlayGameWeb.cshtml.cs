using DAL;
using GameBrain;
using ConsoleUI;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGameWeb : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGameWeb(IConfigRepository configRepository, IGameRepository gameRepository)
    {
        _configRepository = configRepository;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public string? Error { get; set; }
    [BindProperty(SupportsGet = true)] public int ConfigId { get; set; } = default!;
    public EGameAction CurrentAction { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedX { get; set; }
    [BindProperty(SupportsGet = true)] public int SelectedY { get; set; } 
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }
    [BindProperty(SupportsGet = true)] public EGameMode GameMode { get; set; }
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public IActionResult OnGet(int? x, int? y, string? gameType)
    {
        
        OnGetLoadGame(gameType);
        
        if (FinalStageCheck(TicTacTwoBrain) && gameType != nameof(EGameAction.MovePiece))
        {
            CurrentAction = EGameAction.SelectPiece;
            gameType = "SelectPiece";
        }
        
        if (GameMode == EGameMode.AIvsPlayer && TicTacTwoBrain.NextMoveBy == EGamePiece.O)
        {
            TicTacTwoBrain.GridPlacement();
            Error = "It's not your turn!";
            return Page();
        }
        
        // Load game state based on the game type
        if (gameType == nameof(EGameAction.MovePiece) && x != null && y != null)
        {
            if (!TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value))
            {
                Error = "You can't place piece outside of grid!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            TicTacTwoBrain.MovePieceWeb(SelectedX, SelectedY, x.Value, y.Value);
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        }
        else if (x != null && y != null && gameType != nameof(EGameAction.MovePiece) && gameType != nameof(EGameAction.SelectPiece))
        {
            if (!TicTacTwoBrain.MakeAMoveCheck(x.Value, y.Value))
            {
                Error = "You can't place piece outside of grid!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            TicTacTwoBrain.MakeAMove(x.Value, y.Value);
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        } 

        TicTacTwoBrain.GridPlacement();


        if (TicTacTwoBrain.CheckWin())
        {
            return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
        }


        /*
        Console.WriteLine("grid X coords: " + string.Join(", ", TicTacTwoBrain.GridXCoordinates));
        Console.WriteLine("grid Y coords: " + string.Join(", ", TicTacTwoBrain.GridYCoordinates));
        */
        
        return Page();
    }

    public IActionResult OnPost(int? x, int? y, EGameAction action, string? moveDirection)
    {
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        var actionList = new List<string> { "U", "D", "L", "R", "UL", "UR", "DL", "DR" };

        if (action == EGameAction.AIMove)
        {
            TicTacTwoBrain.AiMakeAMove();
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
            if (TicTacTwoBrain.CheckWin())
            {
                return RedirectToPage("./EndPage", new {piece = TicTacTwoBrain.NextMoveBy, gameId = GameId});
            }
            return Page();
        }
        
        CurrentAction = action;
        
        if (!string.IsNullOrEmpty(moveDirection) && actionList.Contains(moveDirection))
        {

            TicTacTwoBrain.MoveGrid(moveDirection);
            if (!TicTacTwoBrain.GridPlacement())
            {
                //Console.WriteLine("here in MoveGridCheckWeb");
                Error = "You can't move grid in this direction!";
                FinalStageCheckAction(TicTacTwoBrain);
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        } 
        else if (CurrentAction == EGameAction.ChoosePiece && x != null && y != null)
        {
            SelectedX = x.Value;
            SelectedY = y.Value;

            CurrentAction = EGameAction.MovePiece;
        } 
        

        if (TicTacTwoBrain.CheckWin())
        {
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
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
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
            CurrentAction = EGameAction.SelectPiece;
        }
        else
        {
            CurrentAction = EGameAction.SelectAction;
        }
    }
}
