dotnet aspnet-codegenerator razorpage -m Game -outDir Pages/Games -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Configuration -outDir Pages/Configurations -dc AppDbContext -udl --referenceScriptLibraries



<!-- Game Board -->
<table class="board">
    @for (var y = 0; y < Model.TicTacTwoBrain.DimY; y++)
    {
        <tr>
            @for (var x = 0; x < Model.TicTacTwoBrain.DimX; x++)
            {
                var cellValue = Model.TicTacTwoBrain.GameBoard[x][y];
                
                // Red cell grid logic (if the cell is part of the "inner board")
                var isRedCell = x < Model.TicTacTwoBrain.DimX - 1 
                                && Model.TicTacTwoBrain.gridXCoordinates.Contains(x + 1)
                                && Model.TicTacTwoBrain.gridYCoordinates.Contains(y + 1);
                
                <td class="@(isRedCell ? "red-cell" : "")" style="width: 50px; height: 50px;">
                    @if (Model.CurrentAction == "SelectPiece")
                    {
                        // Only allow clicking on cells with pieces (x or o)
                        if (cellValue == EGamePiece.X || cellValue == EGamePiece.O)
                        {
                            <a asp-route-x="@x"
                               asp-route-y="@y"
                               asp-route-Action="@Model.CurrentAction"
                               asp-route-GameId="@Model.GameId">
                                @cellValue
                            </a>
                        }
                        else
                        {
                            @cellValue
                        }
                    }
                    else
                    {
                        // Default behavior for other actions
                        if (cellValue == EGamePiece.Empty)
                        {
                            <a asp-route-x="@x"
                               asp-route-y="@y"
                               asp-route-GameId="@Model.GameId">&nbsp;</a>
                        }
                        else
                        {
                            @cellValue
                        }
                    }
                </td>
            }
        </tr>
    }
</table>
