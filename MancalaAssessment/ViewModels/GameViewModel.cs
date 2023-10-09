using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MancalaWPF.Infrastructure.Messages;
using MancalaWPF.Models;
using MancalaGame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MancalaWPF.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        private const int DelayForComputerPlayerThinking = 900;

        private readonly bool _enableTaskDelay = false;
        private readonly IMancala _mancala;

        private bool _allowUserInput = false;
        private MancalaPlayer? _winner;
        private PlayerModel _player1;
        private PlayerModel _player2;

        public bool AllowUserInput
        {
            get => _allowUserInput;
            set
            {
                SetProperty(ref _allowUserInput, value);
                PitCommand.NotifyCanExecuteChanged();
            }
        }
        public MancalaPlayer? Winner
        {
            get => _winner;
            set => SetProperty(ref _winner, value);
        }
        public PlayerModel Player1 => _player1;
        public PlayerModel Player2 => _player2;
        public GameBoardModel GameBoard { get; set; }
        public IRelayCommand PitCommand { get; set; }

        public GameViewModel()
            : this(App.GetService<IMancala>(), App.GetService<IMessenger>(), new GameBoardModel(), true)
        {
        }

        public GameViewModel(IMancala mancala, IMessenger messenger, GameBoardModel gameBoard, bool enableTaskDelay)
        {
            _enableTaskDelay = enableTaskDelay;
            _mancala = mancala;

            GameBoard = gameBoard;
            PitCommand = new RelayCommand<GamePitModel>(ExecutePitCommand, CanExecutePitCommand);

            _player1 = new PlayerModel()
            {
                Name = NewGameMessage.PlayerVsPlayer.Player1Name,
                Player = MancalaPlayer.One,
                IsActive = false
            };
            _player2 = new PlayerModel()
            {
                Name = NewGameMessage.PlayerVsPlayer.Player2Name,
                Player = MancalaPlayer.Two,
                IsActive = false
            };
            _player1.PropertyChanged += OnPlayerPropertyChanged;
            _player2.PropertyChanged += OnPlayerPropertyChanged;

            messenger.Register<NewGameMessage>(this, (_, msg) =>
            {
                Player1.Name = msg.Player1Name;
                Player2.Name = msg.Player2Name;
                Player2.ComputerPlayer = msg.Player2Computer;
                StartNewGame();
            });
            messenger.Register<RestartGameMessage>(this, (_, _) => StartNewGame());

            StartNewGame();
        }

        private async void OnPlayerPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(PlayerModel.IsActive))
            {
                return;
            }

            var playerModel = sender as PlayerModel;
            if (!playerModel!.IsActive || playerModel.ComputerPlayer is null)
            {
                return;
            }

            while (!_mancala.IsFinished && _mancala.Player == playerModel.Player)
            {
                if (_enableTaskDelay)
                {
                    await Task.Delay(DelayForComputerPlayerThinking);
                }

                var nextPitIndex = playerModel.ComputerPlayer.GetNextMovePitIndex(playerModel.Player, _mancala);
                await MakeMoveInGameAsync(playerModel.Player, nextPitIndex);
            }
        }

        private async void ExecutePitCommand(GamePitModel? pit)
        {
            if (pit is null)
            {
                return;
            }
            await MakeMoveInGameAsync(pit.Player, pit.Index);
        }

        private bool CanExecutePitCommand(GamePitModel? pit)
        {
            if (pit is null || pit.Stones == 0)
            {
                return false;
            }

            var playerModel = pit.Player == MancalaPlayer.One ? Player1 : Player2;
            return playerModel.ComputerPlayer is null && playerModel.IsActive && AllowUserInput;
        }

        private async Task MakeMoveInGameAsync(MancalaPlayer player, int pitIndex)
        {
            AllowUserInput = false;

            _mancala.MakeMove(player, pitIndex);

            await GameBoard.UpdateBoardStateAsync(_mancala);

            if (_mancala.IsFinished)
            {
                Winner = _mancala.Player;
                SetActivePlayer(null);
            }
            else
            {
                SetActivePlayer(_mancala.Player);
            }
        }

        private void SetActivePlayer(MancalaPlayer? player)
        {
            Player1.IsActive = player == MancalaPlayer.One;
            Player2.IsActive = player == MancalaPlayer.Two;

            AllowUserInput = player is not null;
        }

        private void StartNewGame()
        {
            _mancala.NewGame();
            GameBoard.InitBoardState(_mancala);
            Winner = null;
            SetActivePlayer(_mancala.Player);
        }
    }
}
