using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Custom : PageModel
{
    
    private readonly IConfigRepository _configRepository;

    public Custom(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    [BindProperty] public string GameMode { get; set; } = default!; 
    [BindProperty] public string ConfName { get; set; } = string.Empty;
    [BindProperty] public int BoardSize { get; set; } = default!;
    [BindProperty] public int GridSize { get; set; } = default!;
    [BindProperty] public int PiecesAmount { get; set; } = default!;
    [BindProperty] public int PiecesToWin { get; set; } = default!;
    [BindProperty] public string? Error { get; set; }
    

    // Gives loadConfig gameType
    public IActionResult OnPost()
    {
        if (!OnCustomGameCheck())
        {
            return Page();
        }
        var gameConfiguration = new GameConfiguration()
        {
            Name = ConfName,
            BoardSizeWidth = BoardSize,
            BoardSizeHeight = BoardSize,
            GridSizeHeight = GridSize,
            GridSizeWidth = GridSize,
            WinCondition = PiecesToWin,
            MovePieceAfterNMoves = (int)(PiecesAmount / 2.0),
            AmountOfPieces = PiecesAmount
        };
        
        var jsonConfStr = System.Text.Json.JsonSerializer.Serialize(gameConfiguration);
        var configId = _configRepository.SaveConfig(jsonConfStr, ConfName);  
        
        if (GameMode == "AIvsAI")
        {
            return RedirectToPage("./PlayGameWebAi", new { ConfigId = configId, gameType = "loadConfig"}); 
        } 
        if (GameMode == "Multiplayer")
        {
            return RedirectToPage("./PlayGameWebPlayer1", new { ConfigId = configId, gameType = "loadConfig"}); 
        } 
        if (GameMode is "PlayerVsPlayer" or "PlayerVsAI")
        {
            return RedirectToPage("./PlayGameWeb", new { ConfigId = configId, gameType = "loadConfig", gameMode = GameMode });
        }
        
        return Page();
    }


    private bool OnCustomGameCheck()
    {
        if (string.IsNullOrWhiteSpace(ConfName))
        {
            Error = "Invalid Config name";
            return false;
        } if (BoardSize < 5)
        {
            Error = "Invalid board size! The board size must be at least 5.";
            return false;
        } if (GridSize < 3 || GridSize >= BoardSize)
        {
            Error = "Invalid grid size! The grid size must be at least 3 and smaller than board size.";
            return false;
        } if (PiecesAmount < 3)
        {
            Error = "Invalid amount of pieces! The amount must be at least 3.";
            return false;
        } if (PiecesToWin >= PiecesAmount || PiecesToWin <= 0 || PiecesToWin > GridSize)
        {
            Error = "Invalid input! Pieces to Win should be less than Amount of Pieces, " +
                    "greater than 0 and less than or equal to grid Size - " + GridSize;
            return false;
        }
        
        return true;
        
    }


}