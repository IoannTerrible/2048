namespace Game2048.Models;

public interface ITileManager
{
    void AddNewTile(int[,] board);
    string GetTileBackground(int value);
    string GetTileForeground(int value);
}

public class TileManager : ITileManager
{
    private readonly Random _random = new();

    public void AddNewTile(int[,] board)
    {
        var emptyCells = new List<(int row, int col)>();
        int size = board.GetLength(0);

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (board[row, col] == 0)
                {
                    emptyCells.Add((row, col));
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            var (row, col) = emptyCells[_random.Next(emptyCells.Count)];
            board[row, col] = _random.Next(10) == 0 ? 4 : 2;
        }
    }

    public string GetTileBackground(int value) => value switch
    {
        0 => "#cdc1b4",
        2 => "#eee4da",
        4 => "#ede0c8",
        8 => "#f2b179",
        16 => "#f59563",
        32 => "#f67c5f",
        64 => "#f65e3b",
        128 => "#edcf72",
        256 => "#edcc61",
        512 => "#edc850",
        1024 => "#edc53f",
        2048 => "#edc22e",
        _ => "#3c3a32"
    };

    public string GetTileForeground(int value) => value <= 4 ? "#776e65" : "#f9f6f2";
}
