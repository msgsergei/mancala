
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MancalaWPF.Infrastructure.Messages;
using MancalaWPF.Models;
using MancalaWPF.ViewModels;
using MancalaGame;
using MancalaGame.ComputerPlayers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace MancalaTests.ViewModelTests
{
    [TestClass]
    public class GameViewModelTests
    {
        private Mock<IMancala> MancalaMock = default!;
        private Mock<GameBoardModel> GameBoardModelMock = default!;
        private IMessenger Messenger = default!;

        private GameViewModel? ViewModel;

        private GameViewModel BuildViewModel()
        {
            ViewModel = new GameViewModel(MancalaMock.Object, Messenger, GameBoardModelMock.Object, false);
            ViewModel.GameBoard = GameBoardModelMock.Object;
            return ViewModel;
        }

        [TestInitialize]
        public void Initialize()
        {
            MancalaMock = new Mock<IMancala>();
            MancalaMock.Setup(x => x.LastMoveUpdates).Returns(new MancalaStateUpdate[]
            {
                new MancalaStateUpdate() { Change = 1, PitIndex = 1, Player = MancalaPlayer.One }
            });
            MancalaMock.Setup(x => x.IsFinished).Returns(false);
            MancalaMock.Setup(x => x.Player).Returns(MancalaPlayer.One);

            Messenger = new StrongReferenceMessenger();

            GameBoardModelMock = new Mock<GameBoardModel>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (ViewModel is not null)
            {
                Messenger.UnregisterAll(ViewModel);
            }
        }

        [TestMethod]
        public void ChangeAllowUserInput_ShouldRaiseNotificationEventForPitCommand()
        {
            // arrange
            var pitCommandMock = new Mock<IRelayCommand>();
            var viewModel = BuildViewModel();
            viewModel.PitCommand = pitCommandMock.Object;

            // act
            viewModel.AllowUserInput = true;

            // assert
            Assert.IsTrue(viewModel.AllowUserInput);
            pitCommandMock.Verify(x => x.NotifyCanExecuteChanged(), Times.Once());
        }

        [TestMethod]
        public void PitCommand_ShouldBeEnabled_WhenActivePlayerIsNotComputer_AndUserInputEnabled_AndPitHasStones()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.AllowUserInput = true;
            viewModel.Player1.IsActive = true;
            viewModel.Player1.ComputerPlayer = null;

            // act
            var canExecute = viewModel.PitCommand
                .CanExecute(new GamePitModel() { Player = MancalaPlayer.One, Index = 0, Stones = 10 });

            // assert
            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void PitCommand_ShouldBeDisabled_WhenThereAreNoStonesInPit()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.AllowUserInput = true;
            viewModel.Player1.IsActive = true;
            viewModel.Player1.ComputerPlayer = null;

            // act
            var canExecute = viewModel.PitCommand
                .CanExecute(new GamePitModel() { Player = MancalaPlayer.One, Index = 0, Stones = 0 });

            // assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void PitCommand_ShouldBeDisabled_WhenUserInputIsDisabled()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.AllowUserInput = false;
            viewModel.Player1.IsActive = true;
            viewModel.Player1.ComputerPlayer = null;

            // act
            var canExecute = viewModel.PitCommand
                .CanExecute(new GamePitModel() { Player = MancalaPlayer.One, Index = 0, Stones = 10 });

            // assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void PitCommand_ShouldBeDisabled_WhenPlayerIsNotActive()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.AllowUserInput = true;
            viewModel.Player1.IsActive = false;
            viewModel.Player1.ComputerPlayer = null;

            // act
            var canExecute = viewModel.PitCommand
                .CanExecute(new GamePitModel() { Player = MancalaPlayer.One, Index = 0, Stones = 10 });

            // assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void PitCommand_ShouldBeDisabled_WhenPlayerIsComputerPlayer()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.AllowUserInput = true;
            viewModel.Player1.IsActive = true;
            viewModel.Player1.ComputerPlayer = new Mock<IMancalaComputerPlayer>().Object;

            // act
            var canExecute = viewModel.PitCommand
                .CanExecute(new GamePitModel() { Player = MancalaPlayer.One, Index = 0, Stones = 10 });

            // assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void PitCommand_ShouldBeDisabledForPitModelIsNull()
        {
            // arrange
            var viewModel = BuildViewModel();

            // act
            var canExecute = viewModel.PitCommand.CanExecute(null);

            // assert
            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void PitCommand_MakeMoveAction_ShouldEndTheGame()
        {
            // arrange
            MancalaMock.Setup(x => x.IsFinished).Returns(true);
            MancalaMock.Setup(x => x.Player).Returns(MancalaPlayer.Two);
            var viewModel = BuildViewModel();

            // act
            viewModel.PitCommand.Execute(
                new GamePitModel() { Index = 2, Player = MancalaPlayer.One, Stones = 3 });

            // assert
            Assert.AreEqual(MancalaPlayer.Two, viewModel.Winner);
            Assert.IsFalse(viewModel.AllowUserInput);
            Assert.IsFalse(viewModel.Player1.IsActive);
            Assert.IsFalse(viewModel.Player2.IsActive);
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.One, 2), Times.Once());
        }

        [TestMethod]
        public void PitCommand_MakeMoveAction_SetAnoterPlayerNextTrun()
        {
            // arrange
            MancalaMock.Setup(x => x.Player).Returns(MancalaPlayer.Two);
            var viewModel = BuildViewModel();

            // act
            viewModel.PitCommand.Execute(
                new GamePitModel() { Index = 2, Player = MancalaPlayer.One, Stones = 3 });

            // assert
            Assert.IsNull(viewModel.Winner);
            Assert.IsTrue(viewModel.AllowUserInput);
            Assert.IsFalse(viewModel.Player1.IsActive);
            Assert.IsTrue(viewModel.Player2.IsActive);
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.One, 2), Times.Once());
        }

        [TestMethod]
        public void PitCommand_MakeMoveActionPlayingWithCpu_ShouldAllowCpuMakeMove()
        {
            // arrange
            var nextPlayer = MancalaPlayer.One;

            var computerPlayer = new Mock<IMancalaComputerPlayer>();
            computerPlayer.Setup(x => x.GetNextMovePitIndex(MancalaPlayer.Two, MancalaMock.Object)).Returns(5);

            MancalaMock.Setup(x => x.Player).Returns(() => nextPlayer);
            MancalaMock.Setup(x => x.MakeMove(It.IsAny<MancalaPlayer>(), It.IsAny<int>()))
                .Callback<MancalaPlayer, int>((player, pitIndex) =>
                {
                    nextPlayer = player == MancalaPlayer.One ? MancalaPlayer.Two : MancalaPlayer.One;
                });

            var viewModel = BuildViewModel();
            viewModel.Player2.ComputerPlayer = computerPlayer.Object;

            // act
            viewModel.PitCommand.Execute(
                new GamePitModel() { Index = 2, Player = MancalaPlayer.One, Stones = 3 });

            // assert
            Assert.IsTrue(viewModel.AllowUserInput);
            Assert.IsTrue(viewModel.Player1.IsActive);
            Assert.IsFalse(viewModel.Player2.IsActive);
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.One, 2), Times.Once());
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.Two, 5), Times.Once());
        }

        [TestMethod]
        public void PitCommand_MakeMoveActionPlayingWithCpu_ShouldHandleConsecutiveMovesForCpu()
        {
            // arrange
            var nextPlayer = MancalaPlayer.One;
            var cpuConsecutiveTurns = 3;

            var computerPlayer = new Mock<IMancalaComputerPlayer>();
            computerPlayer.Setup(x => x.GetNextMovePitIndex(MancalaPlayer.Two, MancalaMock.Object)).Returns(5);

            MancalaMock.Setup(x => x.Player).Returns(() => nextPlayer);
            MancalaMock.Setup(x => x.MakeMove(It.IsAny<MancalaPlayer>(), It.IsAny<int>()))
                .Callback<MancalaPlayer, int>((player, pitIndex) =>
                {
                    if (cpuConsecutiveTurns > 0)
                    {
                        nextPlayer = MancalaPlayer.Two;
                        cpuConsecutiveTurns--;
                    }
                    else
                    {
                        nextPlayer = MancalaPlayer.One;
                    }
                });

            var viewModel = BuildViewModel();
            viewModel.Player2.ComputerPlayer = computerPlayer.Object;

            // act
            viewModel.PitCommand.Execute(
                new GamePitModel() { Index = 2, Player = MancalaPlayer.One, Stones = 3 });

            // assert
            Assert.IsTrue(viewModel.AllowUserInput);
            Assert.IsTrue(viewModel.Player1.IsActive);
            Assert.IsFalse(viewModel.Player2.IsActive);
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.One, 2), Times.Once());
            MancalaMock.Verify(x => x.MakeMove(MancalaPlayer.Two, 5), Times.Exactly(3));
        }

        [TestMethod]
        public void RestartGameMessage_ShouldRestartGame()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.Player1.Name = "alien 1";
            viewModel.Player1.ComputerPlayer = null;
            viewModel.Player2.Name = "alien 2";
            viewModel.Player2.ComputerPlayer = null;
            var message = new RestartGameMessage();

            // act
            Messenger.Send(message);

            // assert
            Assert.IsNull(viewModel.Winner);
            Assert.IsTrue(viewModel.AllowUserInput);
            Assert.IsTrue(viewModel.Player1.IsActive);
            Assert.IsFalse(viewModel.Player2.IsActive);
        }

        [TestMethod]
        public void NewGameMessage_ShouldInitPlayersForPvP()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.Player1.Name = "alien 1";
            viewModel.Player1.ComputerPlayer = null;
            viewModel.Player2.Name = "alien 2";
            viewModel.Player2.ComputerPlayer = null;

            // act
            Messenger.Send(NewGameMessage.PlayerVsPlayer);

            // assert
            Assert.AreEqual(viewModel.Player1.Name, NewGameMessage.PlayerVsPlayer.Player1Name);
            Assert.IsNull(viewModel.Player1.ComputerPlayer);
            Assert.AreEqual(viewModel.Player2.Name, NewGameMessage.PlayerVsPlayer.Player2Name);
            Assert.IsNull(viewModel.Player2.ComputerPlayer);
        }

        [TestMethod]
        public void NewGameMessage_ShouldInitPlayersForPvC()
        {
            // arrange
            var viewModel = BuildViewModel();
            viewModel.Player1.Name = "alien 1";
            viewModel.Player1.ComputerPlayer = null;
            viewModel.Player2.Name = "alien 2";
            viewModel.Player2.ComputerPlayer = null;

            // act
            Messenger.Send(NewGameMessage.PlayerVsComputer);

            // assert
            Assert.AreEqual(viewModel.Player1.Name, NewGameMessage.PlayerVsComputer.Player1Name);
            Assert.IsNull(viewModel.Player1.ComputerPlayer);
            Assert.AreEqual(viewModel.Player2.Name, NewGameMessage.PlayerVsComputer.Player2Name);
            Assert.IsNotNull(viewModel.Player2.ComputerPlayer);
            Assert.IsInstanceOfType(viewModel.Player2.ComputerPlayer, typeof(RandomMancalaComputerPlayer));
        }
    }
}