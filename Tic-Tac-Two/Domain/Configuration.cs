using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Configuration
{
    //Primary Key
    public int Id { get; set; }

    
    [MaxLength(128)] 
    public string Name { get; set; } = default!;

    [MaxLength(10240)] 
    public string State { get; set; }
    
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }    
    
    public ICollection<SaveGame> SaveGames { get; set; }
}