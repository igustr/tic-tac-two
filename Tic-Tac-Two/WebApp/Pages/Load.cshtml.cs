using Azure.Identity;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class Load : PageModel
{
    [BindProperty]
    public string? GameId { get; set; }
    
    private readonly IGameRepository _gameRepository;

    public Load(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }


    [BindProperty(SupportsGet = true)]
    public string UserName { get; set; } = default!;

    public SelectList GamesSelectList { get; set; } = default!;
    
    public IActionResult OnGet()
    {
        var selectListData = _gameRepository.GetSavedGamesNames()
            .Select(name => new SelectListItem { Value = name, Text = name })
            .ToList();

        GamesSelectList = new SelectList(selectListData, "Value", "Text");
        return Page();
    }


    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            // Reload the GamesSelectList in case of validation errors
            var selectListData = _gameRepository.GetSavedGamesNames()
                .Select(name => new SelectListItem { Value = name, Text = name })
                .ToList();

            GamesSelectList = new SelectList(selectListData, "Value", "Text");
            return Page();
        }

        // TODO: Add logic to load the selected game based on GameId
        return RedirectToPage("./PlayGameWeb", new { gameId = GameId });
    }


    }