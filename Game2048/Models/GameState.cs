namespace Game2048.Models;

public enum GameState
{
    Playing,
    Won,
    GameOver
}

public interface IGameStateManager
{
    GameState CurrentState { get; }
    void UpdateState(IGameBoard board);
    string GetStateMessage();
}

public class GameStateManager : IGameStateManager
{
    public GameState CurrentState { get; private set; } = GameState.Playing;

    public void UpdateState(IGameBoard board)
    {
        if (board.HasWon())
        {
            CurrentState = GameState.Won;
        }
        else if (board.IsGameOver())
        {
            CurrentState = GameState.GameOver;
        }
        else
        {
            CurrentState = GameState.Playing;
        }
    }

    public string GetStateMessage()
    {
        return CurrentState switch
        {
            GameState.Playing => "Playing",
            GameState.Won => "You Won!",
            GameState.GameOver => "Game Over!",
            _ => string.Empty
        };
    }
}
