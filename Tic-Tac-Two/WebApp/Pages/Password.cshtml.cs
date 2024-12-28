using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebApp.Pages;

public class Password : PageModel
{
    private readonly ILogger<Password> _logger;
    private readonly IGameRepository _gameRepository;

    public Password(ILogger<Password> logger, IGameRepository gameRepository)
    {
        _logger = logger;
        _gameRepository = gameRepository;
    }

    [BindProperty(SupportsGet = true)] public string? Error { get; set; }

    [BindProperty] public string? InsertedPassword { get; set; }
    [BindProperty(SupportsGet = true)] public int GameId { get; set; }

    public void OnGet()
    {
    }
    
    public IActionResult OnPost()
    {
        InsertedPassword = InsertedPassword?.Trim();
        
        if (!string.IsNullOrWhiteSpace(InsertedPassword) && _gameRepository.GetGameIdByPassword(InsertedPassword) != -1) 
        {
            var gameId = _gameRepository.GetGameIdByPassword(InsertedPassword);
            return RedirectToPage("./PlayGameWebPlayer2", new { GameId = gameId });
        }
        
        Error = "Incorrect password!";
        
        return Page();
    }
}