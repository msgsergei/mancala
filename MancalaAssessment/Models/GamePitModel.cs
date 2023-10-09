using CommunityToolkit.Mvvm.ComponentModel;
using MancalaGame;

namespace MancalaWPF.Models
{
    public class GamePitModel : ObservableObject
    {
        private int _stones = 0;

        public MancalaPlayer Player { get; set; }

        public int Index { get; set; }

        public int Stones
        {
            get => _stones;
            set => SetProperty(ref _stones, value);
        }
    }
}
