using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MancalaWPF.Infrastructure.Messages;
using System.Windows.Input;

namespace MancalaWPF.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;

        public ICommand RestartCommand => new RelayCommand(OnRestart);
        public ICommand NewGameWithPlayerCommand => new RelayCommand(OnNewGameWithPlayer);
        public ICommand NewGameWithComputerCommand => new RelayCommand(OnNewGameWithComputer);

        public MainWindowViewModel()
            : this(App.GetService<IMessenger>())
        {
        }

        public MainWindowViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        private void OnRestart()
        {
            _messenger.Send<RestartGameMessage>(new());
        }

        private void OnNewGameWithPlayer()
        {
            _messenger.Send(NewGameMessage.PlayerVsPlayer);
        }

        private void OnNewGameWithComputer()
        {
            _messenger.Send(NewGameMessage.PlayerVsComputer);
        }
    }
}
