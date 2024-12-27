namespace GameBrain;

public class AIBrain
{
    private readonly GameState _gameState;
    private readonly TicTacTwoBrain _ticTacTwoBrain;
    private int _randomX = default!;
    private int _randomY = default!;
    private static readonly Random RandomGenerator = new Random();

    public AIBrain(GameState gameState, TicTacTwoBrain gameInstance)
    {
        _gameState = gameState;
        _ticTacTwoBrain = gameInstance;
    }
    
    public void AiMakeAMove()
    {
        var gridXCoordinates = _ticTacTwoBrain.gridXCoordinates.ToList();
        var gridYCoordinates = _ticTacTwoBrain.gridYCoordinates.ToList();
        
        do
        {
            var randomIndexX = RandomGenerator.Next(gridXCoordinates.Count);
            var randomIndexY = RandomGenerator.Next(gridYCoordinates.Count);
            
            _randomX = gridXCoordinates[randomIndexX];
            _randomY = gridYCoordinates[randomIndexY];

        } while (!_ticTacTwoBrain.MakeAMoveCheck(_randomX, _randomY));
        
        _gameState.GameBoard[_randomX][_randomY] = _gameState.NextMoveBy;
        
        _gameState.NextMoveBy = _gameState.NextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
    }
    
}