namespace Game2048.Models;

public interface IGameBoard
{
    int Size { get; }
    int Score { get; }
    int GetValue(int row, int col);
    bool Move(Direction direction);
    void Reset();
    bool IsGameOver();
    bool HasWon();
}
