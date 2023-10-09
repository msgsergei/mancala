using CommunityToolkit.Mvvm.ComponentModel;
using MancalaGame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MancalaWPF.Models
{
    public class GameBoardModel : ObservableObject
    {
        private const int DelayForStateUpdate = 200;

        private readonly bool _enableTaskDelay;

        private int _player1Store = 0;
        private int _player2Store = 0;
        private ObservableCollection<GamePitModel> _player1Pits = new();
        private ObservableCollection<GamePitModel> _player2Pits = new();

        public int Player1Store
        {
            get => _player1Store;
            set => SetProperty(ref _player1Store, value);
        }

        public int Player2Store
        {
            get => _player2Store;
            set => SetProperty(ref _player2Store, value);
        }

        public ObservableCollection<GamePitModel> Player1Pits
        {
            get => _player1Pits;
            set => SetProperty(ref _player1Pits, value);
        }

        public ObservableCollection<GamePitModel> Player2Pits
        {
            get => _player2Pits;
            set => SetProperty(ref _player2Pits, value);
        }

        public GameBoardModel()
            : this(true)
        {
        }

        public GameBoardModel(bool enableTaskDelay)
        {
            _enableTaskDelay = enableTaskDelay;
        }

        public virtual void InitBoardState(IMancala mancala)
        {
            var player1State = mancala.GetState(MancalaPlayer.One);
            var player2State = mancala.GetState(MancalaPlayer.Two);

            Player1Store = player1State.Store;
            Player1Pits = new ObservableCollection<GamePitModel>(player1State.Pits
                .Select((stones, index) => new GamePitModel()
                {
                    Index = index,
                    Stones = stones,
                    Player = MancalaPlayer.One,
                }));
            Player2Store = player2State.Store;
            Player2Pits = new ObservableCollection<GamePitModel>(player2State.Pits
                .Select((stones, index) => new GamePitModel()
                {
                    Index = index,
                    Stones = stones,
                    Player = MancalaPlayer.Two,
                }));
        }

        public virtual async Task UpdateBoardStateAsync(IMancala mancala)
        {
            foreach (var update in mancala.LastMoveUpdates)
            {
                if (update.PitIndex is null)
                {
                    if (update.Player == MancalaPlayer.One)
                    {
                        Player1Store += update.Change;
                    }
                    else
                    {
                        Player2Store += update.Change;
                    }
                }
                else
                {
                    if (update.Player == MancalaPlayer.One)
                    {
                        Player1Pits[update.PitIndex.Value].Stones += update.Change;
                    }
                    else
                    {
                        Player2Pits[update.PitIndex.Value].Stones += update.Change;
                    }
                }

                if (_enableTaskDelay)
                {
                    await Task.Delay(DelayForStateUpdate);
                }
            }
        }
    }
}
