using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Game2048.Models;
using Microsoft.Extensions.Logging;

namespace Game2048.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IGameBoard _gameBoard;
    private readonly IGameStateManager _gameStateManager;
    private readonly ITileManager _tileManager;
    private readonly ILogger<MainViewModel> _logger;
    private ObservableCollection<TileViewModel> _tiles;

    [ObservableProperty]
    private int _score;

    [ObservableProperty]
    private string _gameStatus;

    public ObservableCollection<TileViewModel> Tiles => _tiles;

    public MainViewModel(
        IGameBoard gameBoard,
        IGameStateManager gameStateManager,
        ITileManager tileManager,
        ILogger<MainViewModel> logger)
    {
        _gameBoard = gameBoard;
        _gameStateManager = gameStateManager;
        _tileManager = tileManager;
        _logger = logger;
        _tiles = new ObservableCollection<TileViewModel>();
        InitializeBoard();
        UpdateGameStatus();
    }

    private void InitializeBoard()
    {
        _logger.LogInformation("Initializing game board");
        _tiles.Clear();
        for (int row = 0; row < _gameBoard.Size; row++)
        {
            for (int col = 0; col < _gameBoard.Size; col++)
            {
                _tiles.Add(new TileViewModel(_tileManager));
                UpdateTile(row, col);
            }
        }
    }

    private void UpdateBoard()
    {
        for (int row = 0; row < _gameBoard.Size; row++)
        {
            for (int col = 0; col < _gameBoard.Size; col++)
            {
                UpdateTile(row, col);
            }
        }
        Score = _gameBoard.Score;
        UpdateGameStatus();
    }

    private void UpdateTile(int row, int col)
    {
        int index = row * _gameBoard.Size + col;
        _tiles[index].Value = _gameBoard.GetValue(row, col);
    }

    private void UpdateGameStatus()
    {
        _gameStateManager.UpdateState(_gameBoard);
        GameStatus = _gameStateManager.GetStateMessage();
        _logger.LogInformation("Game status updated to: {Status}", GameStatus);
    }

    [RelayCommand]
    private void Move(string direction)
    {
        _logger.LogInformation("Move attempted: {Direction}", direction);
        if (Enum.TryParse<Direction>(direction, out var moveDirection))
        {
            if (_gameBoard.Move(moveDirection))
            {
                UpdateBoard();
                _logger.LogInformation("Move successful, score: {Score}", Score);
            }
        }
    }

    [RelayCommand]
    private void NewGame()
    {
        _logger.LogInformation("Starting new game");
        _gameBoard.Reset();
        Score = 0;
        UpdateBoard();
    }
}

public partial class TileViewModel : ObservableObject
{
    private readonly ITileManager _tileManager;

    public TileViewModel(ITileManager tileManager)
    {
        _tileManager = tileManager;
    }

    [ObservableProperty]
    private int _value;

    public string DisplayValue => Value == 0 ? string.Empty : Value.ToString();
    
    public string Background => _tileManager.GetTileBackground(Value);
    
    public string Foreground => _tileManager.GetTileForeground(Value);

    partial void OnValueChanged(int value)
    {
        OnPropertyChanged(nameof(DisplayValue));
        OnPropertyChanged(nameof(Background));
        OnPropertyChanged(nameof(Foreground));
    }
}
