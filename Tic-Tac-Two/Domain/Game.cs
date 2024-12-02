using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Game : BaseEntity
{
    [MaxLength(10240)]
    public string GameState { get; set; } = default!;
    public int ConfigId { get; set; }
    public int NextMoveBy { get; set; }
    public Configuration? Config { get; set; }
}