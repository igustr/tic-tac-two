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
    [BindProperty] public string ConfName { get; set; } = default!;
    [BindProperty] public int BoardSize { get; set; } = default!;
    [BindProperty] public int GridSize { get; set; } = default!;
    [BindProperty] public int PiecesAmount { get; set; } = default!;
    [BindProperty] public int PiecesToWin { get; set; } = default!;
    [BindProperty] public string? Error { get; set; }
    

    // Gives loadConfig gameType
    public IActionResult OnPost()
    {
        var gameConfiguration = new GameConfiguration()
        {
            Name = ConfName,
            BoardSizeWidth = BoardSize,
            BoardSizeHeight = BoardSize,
            GridSizeHeight = GridSize,
            GridSizeWidth = GridSize,
            WinCondition = PiecesToWin,
            MovePieceAfterNMoves = PiecesAmount / 2,
            AmountOfPieces = PiecesAmount
        };
        Console.WriteLine("in custom, ConfName: " + ConfName);
        
        var jsonConfStr = System.Text.Json.JsonSerializer.Serialize(gameConfiguration);
        var configId = _configRepository.SaveConfig(jsonConfStr, ConfName);  
        
        return RedirectToPage("./PlayGameWeb", new { ConfigId = configId, gameType = "loadConfig" });
    }

    /*
    private bool OnPostCheck()
    {
        if (ConfName == null )
        
        return true;
    }
    */

}