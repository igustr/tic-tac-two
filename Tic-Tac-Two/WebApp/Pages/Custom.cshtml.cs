using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Custom : PageModel
{
    
    private readonly IConfigRepository _configRepository;

    public Custom(IConfigRepository configRepository)
    {
        _configRepository = configRepository;
    }
    public string ConfName { get; set; } = default!;
    public int BoardSize { get; set; } = default!;
    public int GridSize { get; set; } = default!;
    public int PiecesAmount { get; set; } = default!;
    public int PiecesToWin { get; set; } = default!;
    public string? Error { get; set; }
    

    // Gives loadConfig gameType
    public void OnPost()
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
        
        var jsonConfStr = System.Text.Json.JsonSerializer.Serialize(gameConfiguration);
        var configId = _configRepository.SaveConfig(jsonConfStr, ConfName);  
        
        RedirectToPage("./PlayGameWeb", new { ConfigId = configId, gameType = "loadConfig" });
    }

    /*
    private bool OnPostCheck()
    {
        if (ConfName == null )
        
        return true;
    }
    */

}