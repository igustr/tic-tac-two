using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class EndPage : PageModel
{
    private readonly IGameRepository _gameRepository;
    public string? Winner { get; set; }
    
    public EndPage(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    public void OnGet(string? piece, int? gameId)
    {
        piece = piece == "X" ? "O" : "X";

        Winner = piece;
        
        Console.WriteLine("gameid endpage: " + gameId);

        if (gameId.HasValue)
        {
            _gameRepository.DeleteGame(gameId.Value);  
        }
    }
    
}
