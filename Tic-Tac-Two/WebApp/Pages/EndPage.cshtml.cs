using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class EndPage : PageModel
{
    private readonly IGameRepository _gameRepository;
    [BindProperty(SupportsGet = true)] public int GameId { get; set; } = default!;
    public string? Winner { get; set; }
    
    public EndPage(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    public void OnGet(string? piece)
    {
        piece = piece == "X" ? "O" : "X";

        Winner = piece;
        
        
        Console.WriteLine("game id in endpage: " + GameId);
        _gameRepository.DeleteGame(GameId);
    }
    
}
