using System;
using System.Linq;

namespace Game2048.Models
{
    public class GameBoard : IGameBoard
    {
        private readonly int[,] board;
        private readonly ITileManager _tileManager;

        public int Size => 4;
        public int Score { get; private set; }

        public GameBoard(ITileManager tileManager)
        {
            _tileManager = tileManager;
            board = new int[Size, Size];
            Reset();
        }

        public int GetValue(int row, int col) => board[row, col];

        public void Reset()
        {
            Array.Clear(board, 0, board.Length);
            Score = 0;
            _tileManager.AddNewTile(board);
            _tileManager.AddNewTile(board);
        }

        public bool Move(Direction direction)
        {
            bool moved = direction switch
            {
                Direction.Up => MoveUp(),
                Direction.Down => MoveDown(),
                Direction.Left => MoveLeft(),
                Direction.Right => MoveRight(),
                _ => false
            };

            if (moved)
            {
                _tileManager.AddNewTile(board);
            }

            return moved;
        }

        private bool MoveLeft()
        {
            bool moved = false;
            for (int row = 0; row < Size; row++)
            {
                for (int col = 1; col < Size; col++)
                {
                    if (board[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol > 0 && (board[row, currentCol - 1] == 0 || board[row, currentCol - 1] == board[row, currentCol]))
                        {
                            if (board[row, currentCol - 1] == board[row, currentCol])
                            {
                                board[row, currentCol - 1] *= 2;
                                Score += board[row, currentCol - 1];
                                board[row, currentCol] = 0;
                                moved = true;
                                break;
                            }
                            else if (board[row, currentCol - 1] == 0)
                            {
                                board[row, currentCol - 1] = board[row, currentCol];
                                board[row, currentCol] = 0;
                                currentCol--;
                                moved = true;
                            }
                        }
                    }
                }
            }
            return moved;
        }

        private bool MoveRight()
        {
            bool moved = false;
            for (int row = 0; row < Size; row++)
            {
                for (int col = Size - 2; col >= 0; col--)
                {
                    if (board[row, col] != 0)
                    {
                        int currentCol = col;
                        while (currentCol < Size - 1 && (board[row, currentCol + 1] == 0 || board[row, currentCol + 1] == board[row, currentCol]))
                        {
                            if (board[row, currentCol + 1] == board[row, currentCol])
                            {
                                board[row, currentCol + 1] *= 2;
                                Score += board[row, currentCol + 1];
                                board[row, currentCol] = 0;
                                moved = true;
                                break;
                            }
                            else if (board[row, currentCol + 1] == 0)
                            {
                                board[row, currentCol + 1] = board[row, currentCol];
                                board[row, currentCol] = 0;
                                currentCol++;
                                moved = true;
                            }
                        }
                    }
                }
            }
            return moved;
        }

        private bool MoveUp()
        {
            bool moved = false;
            for (int col = 0; col < Size; col++)
            {
                for (int row = 1; row < Size; row++)
                {
                    if (board[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow > 0 && (board[currentRow - 1, col] == 0 || board[currentRow - 1, col] == board[currentRow, col]))
                        {
                            if (board[currentRow - 1, col] == board[currentRow, col])
                            {
                                board[currentRow - 1, col] *= 2;
                                Score += board[currentRow - 1, col];
                                board[currentRow, col] = 0;
                                moved = true;
                                break;
                            }
                            else if (board[currentRow - 1, col] == 0)
                            {
                                board[currentRow - 1, col] = board[currentRow, col];
                                board[currentRow, col] = 0;
                                currentRow--;
                                moved = true;
                            }
                        }
                    }
                }
            }
            return moved;
        }

        private bool MoveDown()
        {
            bool moved = false;
            for (int col = 0; col < Size; col++)
            {
                for (int row = Size - 2; row >= 0; row--)
                {
                    if (board[row, col] != 0)
                    {
                        int currentRow = row;
                        while (currentRow < Size - 1 && (board[currentRow + 1, col] == 0 || board[currentRow + 1, col] == board[currentRow, col]))
                        {
                            if (board[currentRow + 1, col] == board[currentRow, col])
                            {
                                board[currentRow + 1, col] *= 2;
                                Score += board[currentRow + 1, col];
                                board[currentRow, col] = 0;
                                moved = true;
                                break;
                            }
                            else if (board[currentRow + 1, col] == 0)
                            {
                                board[currentRow + 1, col] = board[currentRow, col];
                                board[currentRow, col] = 0;
                                currentRow++;
                                moved = true;
                            }
                        }
                    }
                }
            }
            return moved;
        }

        public bool IsGameOver()
        {
            // Check for empty cells
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (board[row, col] == 0)
                        return false;
                }
            }

            // Check for possible merges
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (col < Size - 1 && board[row, col] == board[row, col + 1])
                        return false;
                    if (row < Size - 1 && board[row, col] == board[row + 1, col])
                        return false;
                }
            }

            return true;
        }

        public bool HasWon()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (board[row, col] == 2048)
                        return true;
                }
            }
            return false;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
