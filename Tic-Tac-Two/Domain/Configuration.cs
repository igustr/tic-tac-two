using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Configuration : BaseEntity
{
    //Primary Key
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; }
    public int BoardSizeHeight { get; set; }
    public int WinCondition { get; set; }
    public int MovePieceAfterNMoves { get; set; }
    public int AmountOfPieces { get; set; }
    public int GridSizeHeight { get; set; }
    public int GridSizeWidth { get; set; }
}