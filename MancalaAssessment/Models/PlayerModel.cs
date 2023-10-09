using CommunityToolkit.Mvvm.ComponentModel;
using MancalaGame;
using MancalaGame.ComputerPlayers;

namespace MancalaWPF.Models
{
    public class PlayerModel : ObservableObject
    {
        private bool _isActive = false;
        private string _name = default!;
        private IMancalaComputerPlayer? _computerPlayer;

        public MancalaPlayer Player { get; init; } = default!;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public IMancalaComputerPlayer? ComputerPlayer
        {
            get => _computerPlayer;
            set => SetProperty(ref _computerPlayer, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
    }
}
