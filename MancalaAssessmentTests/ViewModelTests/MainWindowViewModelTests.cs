using CommunityToolkit.Mvvm.Messaging;
using MancalaWPF.Infrastructure.Messages;
using MancalaWPF.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MancalaTests.ViewModelTests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TypeMatcher]
        private sealed class IsAnyToken : ITypeMatcher, IEquatable<IsAnyToken>
        {
            public bool Matches(Type typeArgument) => true;
            public bool Equals(IsAnyToken? other) => throw new NotImplementedException();
        }

        private Mock<IMessenger> MessengerMock = default!;

        private MainWindowViewModel BuildViewModel()
        {
            return new MainWindowViewModel(MessengerMock.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            MessengerMock = new Mock<IMessenger>();
        }

        [TestMethod]
        public void RestartCommand_ShouldSendRestartGameMessage()
        {
            // arrange
            var viewModel = BuildViewModel();

            // act
            viewModel.RestartCommand.Execute(null);

            // assert
            MessengerMock.Verify(x => x.Send(It.IsAny<RestartGameMessage>(), It.IsAny<IsAnyToken>()), Times.Once());
        }

        [TestMethod]
        public void NewGameWithPlayerCommand_ShouldSendDefaultPvPNewGameMessage()
        {
            // arrange
            var viewModel = BuildViewModel();

            // act
            viewModel.NewGameWithPlayerCommand.Execute(null);

            // assert
            MessengerMock.Verify(x => x.Send(NewGameMessage.PlayerVsPlayer, It.IsAny<IsAnyToken>()), Times.Once());
        }

        [TestMethod]
        public void NewGameWithPlayerCommand_ShouldSendDefaultPvCNewGameMessage()
        {
            // arrange
            var viewModel = BuildViewModel();

            // act
            viewModel.NewGameWithComputerCommand.Execute(null);

            // assert
            MessengerMock.Verify(x => x.Send(NewGameMessage.PlayerVsComputer, It.IsAny<IsAnyToken>()), Times.Once());
        }
    }
}
