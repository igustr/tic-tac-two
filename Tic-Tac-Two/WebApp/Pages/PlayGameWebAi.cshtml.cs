using DAL;
using GameBrain;
using ConsoleUI;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class PlayGameWebAi : PageModel
{
    private readonly IConfigRepository _configRepository;
    private readonly IGameRepository _gameRepository;

    public PlayGameWebAi(IConfigRepository configRepository, IGameRepository gameRepository)
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
    public TicTacTwoBrain TicTacTwoBrain { get; set; } = default!;

    public IActionResult OnGet(int? x, int? y, string? gameType)
    {

        // Load Game Instance from DB or Create new one
        OnGetLoadGame(gameType);
        
        // If AI turn
        if (TicTacTwoBrain.NextMoveBy == EGamePiece.O)
        {
            TicTacTwoBrain.AiMakeAMove();
            return Page();
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
    

    public IActionResult OnPost(int? x, int? y, string action)
    {
        var gameState = _gameRepository.LoadGame(GameId);
        TicTacTwoBrain = new TicTacTwoBrain(gameState);
        var actionList = new List<string> { "U", "D", "L", "R", "UL", "UR", "DL", "DR" };
        
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
                GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
                return Page();
            }
            FinalStageCheckAction(TicTacTwoBrain);
            GameId = _gameRepository.SaveGame(TicTacTwoBrain.GetGameStateJson(), GameId, "gameName");
        } 
        else if (CurrentAction == "ChoosePiece" && x != null && y != null)
        {
            SelectedX = x.Value;
            SelectedY = y.Value;

            CurrentAction = "MovePiece";
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
        //Console.WriteLine("game id: " + GameId);
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

    private void AiMove()
    {
        
    }
}
