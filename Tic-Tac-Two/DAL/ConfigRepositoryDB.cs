using GameBrain;

namespace DAL;

public class ConfigRepositoryDB : IConfigRepository
{
    private readonly DAL.AppDbContext _context;
    
    public ConfigRepositoryDB(AppDbContext context)
    {
        _context = context;
    }
    
    public List<string> GetConfigurationNames()
    {
        return _context.Configurations
            .OrderBy(c => c.Name)
            .Select(c => c.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        var data = _context.Configurations.First(c => c.Name == name);
        var res = new GameConfiguration()
        {
            Name = data.Name,
            BoardSizeWidth = data.BoardSizeWidth,
            BoardSizeHeight = data.BoardSizeHeight,
            GridSizeHeight = data.BoardSizeHeight,
            GridSizeWidth = data.GridSizeWidth,
            WinCondition = data.WinCondition,
            MovePieceAfterNMoves = data.MovePieceAfterNMoves,
            AmountOfPieces = data.AmountOfPieces
        };
        return res;
    }

    public void SaveConfig(string jsonConfigString)
    {
        throw new NotImplementedException();
    }
}