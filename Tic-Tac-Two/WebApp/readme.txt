dotnet aspnet-codegenerator razorpage -m Game -outDir Pages/Games -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Configuration -outDir Pages/Configurations -dc AppDbContext -udl --referenceScriptLibraries



<!-- Game Board -->
<table class="board">
    @for (var y = 0; y < Model.TicTacTwoBrain.DimY; y++)
    {
        <tr>
            @for (var x = 0; x < Model.TicTacTwoBrain.DimX; x++)
            {
                <td class="@(Model.TicTacTwoBrain.GridXCoordinates.Contains(x + 1) && Model.TicTacTwoBrain.GridYCoordinates.Contains(y + 1) ? "red-cell" : "") @(Model.SelectedPiece.HasValue && Model.SelectedPiece.Value == (x, y) ? "highlighted" : "")">
                    @if (Model.TicTacTwoBrain.GameBoard[x][y] == EGamePiece.Empty)
                    {
                        <a asp-route-x="@x" asp-route-y="@y" 
                           asp-route-Action="@Model.CurrentAction" 
                           asp-route-GameId="@Model.GameId">&nbsp;</a>
                    }
                    else
                    {
                        <a asp-route-x="@x" asp-route-y="@y" 
                           asp-route-Action="@Model.CurrentAction" 
                           asp-route-GameId="@Model.GameId">
                            @(Model.TicTacTwoBrain.GameBoard[x][y])
                        </a>
                    }
                </td>
            }
        </tr>
    }
</table>

<!-- Action Buttons -->
@if (Model.TicTacTwoBrain.GetPiecesOnBoardCount().Item2 >= Model.TicTacTwoBrain.MovePieceAfterNMoves)
{
    @if (Model.CurrentAction == "SelectAction" || string.IsNullOrEmpty(Model.CurrentAction))
    {
        <form method="post">
            <div style="text-align: center; margin-top: 20px;">
                <button type="submit" class="btn btn-primary" name="Action" value="SelectPiece">Move Piece</button>
            </div>
        </form>
    }
}

-------------------------------------------------------------------------------------------------------------------------

<!-- Display Game State Based on Current Action -->
@if (Model.CurrentAction == "SelectPiece")
{
    <p style="text-align: center;">Select a piece to move:</p>
}
else if (Model.CurrentAction == "MovePiece" && Model.SelectedPiece.HasValue)
{
    <p style="text-align: center;">
        Selected Piece: (@Model.SelectedPiece.Value.X, @Model.SelectedPiece.Value.Y)<br>
        Select a target cell for the piece.
    </p>
}

<!-- Game Board -->
<table class="board">
    @for (var y = 0; y < Model.TicTacTwoBrain.DimY; y++)
    {
        <tr>
            @for (var x = 0; x < Model.TicTacTwoBrain.DimX; x++)
            {
                <td class="@(Model.SelectedPiece.HasValue && Model.SelectedPiece.Value == (x, y) ? "highlighted" : "")">
                    @if (Model.TicTacTwoBrain.GameBoard[x][y] == EGamePiece.Empty)
                    {
                        <a asp-route-x="@x" asp-route-y="@y" 
                           asp-route-Action="@Model.CurrentAction" 
                           asp-route-GameId="@Model.GameId">&nbsp;</a>
                    }
                    else
                    {
                        <a asp-route-x="@x" asp-route-y="@y" 
                           asp-route-Action="@Model.CurrentAction" 
                           asp-route-GameId="@Model.GameId">
                            @(Model.TicTacTwoBrain.GameBoard[x][y])
                        </a>
                    }
                </td>
            }
        </tr>
    }
</table>

<!-- Action Buttons -->
@if (Model.TicTacTwoBrain.GetPiecesOnBoardCount().Item2 >= Model.TicTacTwoBrain.MovePieceAfterNMoves)
{
    @if (Model.CurrentAction == "SelectAction" || string.IsNullOrEmpty(Model.CurrentAction))
    {
        <form method="post">
            <div style="text-align: center; margin-top: 20px;">
                <button type="submit" class="btn btn-primary" name="Action" value="SelectPiece">Move Piece</button>
            </div>
        </form>
    }
}
