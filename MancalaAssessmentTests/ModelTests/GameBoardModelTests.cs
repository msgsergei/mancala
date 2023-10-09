using CommunityToolkit.Mvvm.Messaging;
using MancalaWPF.Models;
using MancalaWPF.ViewModels;
using MancalaGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MancalaTests.ModelTests
{
    [TestClass]
    public class GameBoardModelTests
    {
        private Mock<IMancala> MancalaMock = new Mock<IMancala>();

        private GameBoardModel BuildGameBoardModel()
        {
            return new GameBoardModel(false);
        }

        [TestInitialize]
        public void Initialize()
        {
            MancalaMock = new Mock<IMancala>();
            MancalaMock.Setup(x => x.GetState(MancalaPlayer.One))
                .Returns(new MancalaPlayerState(5, new int[] { 1, 2, 3, 4, 5, 6 }));
            MancalaMock.Setup(x => x.GetState(MancalaPlayer.Two))
                .Returns(new MancalaPlayerState(1, new int[] { 6, 5, 4, 3, 2, 1 }));

            MancalaMock.Setup(x => x.LastMoveUpdates)
                .Returns(new List<MancalaStateUpdate>()
                {
                    new MancalaStateUpdate() { Change = 2, PitIndex = null, Player = MancalaPlayer.One },
                    new MancalaStateUpdate() { Change = 2, PitIndex = null, Player = MancalaPlayer.Two },
                    new MancalaStateUpdate() { Change = -6, PitIndex = 5, Player = MancalaPlayer.One },
                    new MancalaStateUpdate() { Change = -2, PitIndex = 1, Player = MancalaPlayer.Two },
                });
        }

        [TestMethod]
        public void ShouldInitState_WithValidStonesCount()
        {
            // arrange
            var gameBoard = BuildGameBoardModel();

            // act
            gameBoard.InitBoardState(MancalaMock.Object);

            // assert
            Assert.AreEqual(5, gameBoard.Player1Store);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, gameBoard.Player1Pits.Select(x => x.Stones).ToArray());
            Assert.AreEqual(1, gameBoard.Player2Store);
            CollectionAssert.AreEqual(new int[] { 6, 5, 4, 3, 2, 1 }, gameBoard.Player2Pits.Select(x => x.Stones).ToArray());
        }

        [TestMethod]
        public void ShouldInitState_WithValidIndices()
        {
            // arrange
            var gameBoard = BuildGameBoardModel();

            // act
            gameBoard.InitBoardState(MancalaMock.Object);

            // assert
            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5 }, gameBoard.Player1Pits.Select(x => x.Index).ToArray());
            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5 }, gameBoard.Player2Pits.Select(x => x.Index).ToArray());
        }

        [TestMethod]
        public void ShouldInitState_WithValidPitsPlayerNumbers()
        {
            // arrange
            var gameBoard = BuildGameBoardModel();

            // act
            gameBoard.InitBoardState(MancalaMock.Object);

            // assert
            Assert.AreEqual(true, gameBoard.Player1Pits.All(x => x.Player == MancalaPlayer.One));
            Assert.AreEqual(true, gameBoard.Player2Pits.All(x => x.Player == MancalaPlayer.Two));
        }

        [TestMethod]
        public async Task ShouldUpdateState_WithLatestMancalaChangesFromLastMove()
        {
            // arrange
            var gameBoard = BuildGameBoardModel();
            gameBoard.InitBoardState(MancalaMock.Object);

            // act
            await gameBoard.UpdateBoardStateAsync(MancalaMock.Object);

            // assert
            Assert.AreEqual(7, gameBoard.Player1Store);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4, 5, 0 }, gameBoard.Player1Pits.Select(x => x.Stones).ToArray());
            Assert.AreEqual(3, gameBoard.Player2Store);
            CollectionAssert.AreEqual(new int[] { 6, 3, 4, 3, 2, 1 }, gameBoard.Player2Pits.Select(x => x.Stones).ToArray());
        }
    }
}
