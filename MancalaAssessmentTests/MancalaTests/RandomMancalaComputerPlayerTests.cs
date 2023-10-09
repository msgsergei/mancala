using MancalaGame;
using MancalaGame.ComputerPlayers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace MancalaTests.MancalaTests
{
    [TestClass]
    public class RandomMancalaComputerPlayerTests
    {
        [TestMethod]
        public void GetNextMovePitIndex_ShouldPickValidPitIndex_IgnoringEmptyPits()
        {
            // arrange
            var randomMock = new Mock<Random>();
            randomMock.Setup(x => x.Next(It.IsAny<int>())).Returns(1);
            var stateMock = new Mock<IMancalaState>();
            stateMock.Setup(x => x.GetState(MancalaPlayer.One))
                .Returns(new MancalaPlayerState(10, new int[] { 1, 0, 3, 0, 0, 6 }));
            var cpu = new RandomMancalaComputerPlayer(randomMock.Object);

            // act
            var cpuSelectedPitIndex = cpu.GetNextMovePitIndex(MancalaPlayer.One, stateMock.Object);

            // assert
            Assert.AreEqual(2, cpuSelectedPitIndex);
            randomMock.Verify(x => x.Next(3), Times.Once());
            randomMock.Verify(x => x.Next(It.IsIn(1, 2, 4, 5, 6)), Times.Never());
        }
    }
}
