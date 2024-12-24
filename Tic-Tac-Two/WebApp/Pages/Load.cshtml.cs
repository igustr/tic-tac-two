using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class Load : PageModel
{
    private readonly IGameRepository _gameRepository;

    public Load(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    [BindProperty]
    public string GameName { get; set; } = string.Empty;

    public SelectList GamesSelectList { get; private set; } = default!;

    public IActionResult OnGet()
    {
        LoadGamesList();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(GameName))
        {
            ModelState.AddModelError(nameof(GameName), "Please select a valid game.");
            LoadGamesList();
            return Page();
        }

        var gameId = _gameRepository.GetIdByName(GameName);
        return RedirectToPage("./PlayGameWeb", new { GameId = gameId, gameType = "load" });
    }

    private void LoadGamesList()
    {
        var savedGames = _gameRepository.GetSavedGamesNames();
        GamesSelectList = new SelectList(
            savedGames.Select(name => new SelectListItem { Value = name, Text = name }),
            "Value", "Text"
        );
    }
}