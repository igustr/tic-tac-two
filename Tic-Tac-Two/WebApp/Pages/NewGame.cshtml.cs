using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    [BindProperty] public string GameMode { get; set; } = default!;  
    [BindProperty] public string GameType { get; set; } = default!;  

    public IActionResult OnPost()
    {

        if (string.IsNullOrEmpty(GameMode) || string.IsNullOrEmpty(GameType))
        {
            return Page();
        }

        if (GameType == "default")
        {
            if (GameMode == "AIvsAI")
            {
                return RedirectToPage("./PlayGameWebAi", new { gameType = "new"}); 
            } 
            if (GameMode == "Multiplayer")
            {
                return RedirectToPage("./PlayGameWebPlayer1", new { gameType = "new"}); 
            } 
            if (GameMode is "PlayerVsPlayer" or "PlayerVsAI")
            {
                return RedirectToPage("./PlayGameWeb", new { gameType = "new", gameMode = GameMode });
            }
        }
        
        if (GameType == "custom")
        {
            return RedirectToPage("./Custom", new { gameMode = GameMode });
        }

        return Page(); 
    }
}