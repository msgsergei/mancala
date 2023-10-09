using MancalaGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MancalaTests.MancalaTests
{
    [TestClass]
    public class MancalaTests
    {
        [TestMethod]
        public void Constructor_ShouldSetupValidDefaultState()
        {
            // Arrange
            var mancala = new Mancala();

            // assert
            Assert.AreEqual(0, mancala.GetState(MancalaPlayer.One).Store);
            Assert.AreEqual(6, mancala.GetState(MancalaPlayer.One).Pits.Count);
            Assert.AreEqual(true, mancala.GetState(MancalaPlayer.One).Pits.All(x => x == 4));
            Assert.AreEqual(0, mancala.GetState(MancalaPlayer.Two).Store);
            Assert.AreEqual(6, mancala.GetState(MancalaPlayer.Two).Pits.Count);
            Assert.AreEqual(true, mancala.GetState(MancalaPlayer.Two).Pits.All(x => x == 4));
        }

        [TestMethod]
        public void Constructor_ShouldSetupValidExplicitState()
        {
            // arrange
            var mancala = new Mancala(
                10, new int[] { 1, 2, 3, 4, 5, 6 },
                90, new int[] { 10, 20, 30, 40, 50, 60 });
            var state1 = mancala.GetState(MancalaPlayer.One);
            var state2 = mancala.GetState(MancalaPlayer.Two);

            // assert
            Assert.AreEqual(10, state1.Store);
            Assert.AreEqual(6, state1.Pits.Count);
            Assert.AreEqual(1, state1.Pits[0]);
            Assert.AreEqual(2, state1.Pits[1]);
            Assert.AreEqual(3, state1.Pits[2]);
            Assert.AreEqual(4, state1.Pits[3]);
            Assert.AreEqual(5, state1.Pits[4]);
            Assert.AreEqual(6, state1.Pits[5]);

            Assert.AreEqual(90, state2.Store);
            Assert.AreEqual(6, state2.Pits.Count);
            Assert.AreEqual(10, state2.Pits[0]);
            Assert.AreEqual(20, state2.Pits[1]);
            Assert.AreEqual(30, state2.Pits[2]);
            Assert.AreEqual(40, state2.Pits[3]);
            Assert.AreEqual(50, state2.Pits[4]);
            Assert.AreEqual(60, state2.Pits[5]);
        }

        [TestMethod]
        public void NewGame_ShouldResetGameToDefaultState()
        {
            // arrange
            var mancala = new Mancala();
            var state1 = mancala.GetState(MancalaPlayer.One);
            var state2 = mancala.GetState(MancalaPlayer.Two);

            // act
            state1.PutOneStoneIntoPit(0);
            state1.PutOneStoneIntoPit(1);
            state1.PutOneStoneIntoPit(2);
            state1.PutOneStoneIntoPit(3);
            state1.PutOneStoneIntoPit(4);
            state1.PutOneStoneIntoPit(5);
            state2.PutOneStoneIntoPit(0);
            state2.PutOneStoneIntoPit(1);
            state2.PutOneStoneIntoPit(2);
            state2.PutOneStoneIntoPit(3);
            state2.PutOneStoneIntoPit(4);
            state2.PutOneStoneIntoPit(5);
            mancala.NewGame();

            // assert
            Assert.AreEqual(0, mancala.GetState(MancalaPlayer.One).Store);
            Assert.AreEqual(6, mancala.GetState(MancalaPlayer.One).Pits.Count);
            Assert.AreEqual(true, mancala.GetState(MancalaPlayer.One).Pits.All(x => x == 4));

            Assert.AreEqual(0, mancala.GetState(MancalaPlayer.Two).Store);
            Assert.AreEqual(6, mancala.GetState(MancalaPlayer.Two).Pits.Count);
            Assert.AreEqual(true, mancala.GetState(MancalaPlayer.Two).Pits.All(x => x == 4));
        }

        [TestMethod]
        public void MakeMove_MoveStonesIntoOwnPitsOnly_ShouldUpdatesStateAndPassTurn()
        {
            // arrange
            var game = new Mancala(
                0, new int[] { 4, 4, 4, 4, 4, 4 },
                0, new int[] { 4, 4, 4, 4, 4, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 0);

            // assert
            Assert.AreEqual(MancalaPlayer.Two, game.Player);
            Assert.IsFalse(game.IsFinished);
            Assert.AreEqual(0, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 0, 5, 5, 5, 5, 4 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(0, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 4, 4, 4, 4, 4, 4 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void MakeMove_MoveStonesIntoOwnPitsAndIntoOpponentsPit_ShouldUpdatesStateAndPassTurn()
        {
            // arrange
            var game = new Mancala(
                0, new int[] { 4, 4, 4, 4, 4, 4 },
                0, new int[] { 4, 4, 4, 4, 4, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 4);

            // assert
            Assert.AreEqual(MancalaPlayer.Two, game.Player);
            Assert.IsFalse(game.IsFinished);
            Assert.AreEqual(1, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 4, 4, 4, 4, 0, 5 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(0, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 5, 5, 4, 4, 4, 4 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void MakeMove_MoveStonesIntoOwnPitsAndFinishInOwnStore_ShouldUpdateStateAndTakeAnotherTurn()
        {
            // arrange
            var game = new Mancala(
                0, new int[] { 4, 4, 4, 4, 4, 4 },
                0, new int[] { 4, 4, 4, 4, 4, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 2);

            // assert
            Assert.AreEqual(MancalaPlayer.One, game.Player);
            Assert.IsFalse(game.IsFinished);
            Assert.AreEqual(1, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 4, 4, 0, 5, 5, 5 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(0, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 4, 4, 4, 4, 4, 4 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void MakeMove_MoveStonesInRoundtripAndRetunBackToOwnPits_ShouldUpdateStateAndNotUpdateOpponentsStore()
        {
            // arrange
            var game = new Mancala(
                0, new int[] { 4, 4, 4, 4, 10, 4 },
                0, new int[] { 4, 4, 4, 4, 4, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 4);

            // assert
            Assert.AreEqual(MancalaPlayer.Two, game.Player);
            Assert.IsFalse(game.IsFinished);
            Assert.AreEqual(1, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 5, 5, 4, 4, 0, 5 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(0, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 5, 5, 5, 5, 5, 5 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void MakeMove_MoveStonesIntoOwnPitsAndDropLastToEmptyPit_ShouldUpdatesStatePutStonesFromOwnAndOpponentsOppositePitIntoStore()
        {
            // arrange
            var game = new Mancala(
                5, new int[] { 5, 5, 4, 5, 4, 0 },
                7, new int[] { 3, 4, 4, 0, 5, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 0);

            // assert
            Assert.AreEqual(MancalaPlayer.Two, game.Player);
            Assert.IsFalse(game.IsFinished);
            Assert.AreEqual(9, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 0, 6, 5, 6, 5, 0 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(7, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 0, 4, 4, 0, 5, 4 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void EndGame_ShouldMoveAllStonesFromPitsToStores()
        {
            // arrange
            var game = new Mancala(
                27, new int[] { 0, 1, 0, 0, 0, 0 },
                5, new int[] { 0, 5, 0, 5, 5, 0 });

            // act
            game.MakeMove(MancalaPlayer.One, 1);

            // assert
            Assert.AreEqual(MancalaPlayer.One, game.Player);
            Assert.IsTrue(game.IsFinished);
            Assert.AreEqual(33, game.GetState(MancalaPlayer.One).Store);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 0, 0, 0 }, game.GetState(MancalaPlayer.One).Pits.ToList());
            Assert.AreEqual(15, game.GetState(MancalaPlayer.Two).Store);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 0, 0, 0 }, game.GetState(MancalaPlayer.Two).Pits.ToList());
        }

        [TestMethod]
        public void MakeMove_RemovedLastActivePlayerStone_ShouldTriggerEndGame()
        {
            // arrange
            var game = new Mancala(
                28, new int[] { 0, 0, 0, 0, 0, 1 },
                6, new int[] { 2, 0, 2, 0, 5, 4 });

            // act
            game.MakeMove(MancalaPlayer.One, 5);

            // assert
            Assert.AreEqual(MancalaPlayer.One, game.Player);
            Assert.IsTrue(game.IsFinished);
            Assert.AreEqual(29, game.GetState(MancalaPlayer.One).Store);
            Assert.AreEqual(19, game.GetState(MancalaPlayer.Two).Store);
        }

        [TestMethod]
        public void MakeMove_RemovedLastOpponentPlayerStone_ShouldTriggerEndGame()
        {
            // arrange
            var game = new Mancala(
                10, new int[] { 0, 1, 0, 4, 0, 1 },
                27, new int[] { 0, 0, 0, 5, 0, 0 });

            // act
            game.MakeMove(MancalaPlayer.One, 1);

            // assert
            Assert.AreEqual(MancalaPlayer.Two, game.Player);
            Assert.IsTrue(game.IsFinished);
            Assert.AreEqual(21, game.GetState(MancalaPlayer.One).Store);
            Assert.AreEqual(27, game.GetState(MancalaPlayer.Two).Store);
        }

        [TestMethod]
        public void MakeMove_RemovedLastOwnAndOpponentsPlayerStone_ShouldTriggerEndGame()
        {
            // arrange
            var game = new Mancala(
                27, new int[] { 0, 1, 0, 0, 0, 0 },
                15, new int[] { 0, 0, 0, 5, 0, 0 });

            // act
            game.MakeMove(MancalaPlayer.One, 1);

            // assert
            Assert.AreEqual(MancalaPlayer.One, game.Player);
            Assert.IsTrue(game.IsFinished);
            Assert.AreEqual(33, game.GetState(MancalaPlayer.One).Store);
            Assert.AreEqual(15, game.GetState(MancalaPlayer.Two).Store);
        }

        [TestMethod]
        [DataRow(30, 16, MancalaPlayer.One)]
        [DataRow(16, 30, MancalaPlayer.Two)]
        public void EndGameScore_WinnerMustBePlayerWithMoreStonesInStore(int player1store, int player2store, MancalaPlayer expectedWinner)
        {
            // arrange
            var game = new Mancala(
                player1store, new int[] { 0, 0, 0, 0, 0, 1 },
                player2store, new int[] { 0, 0, 0, 0, 0, 1 });

            // act
            game.MakeMove(MancalaPlayer.One, 5);

            // assert
            Assert.AreEqual(expectedWinner, game.Player);
            Assert.IsTrue(game.IsFinished);
        }

        [TestMethod]
        [DataRow(MancalaPlayer.One)]
        [DataRow(MancalaPlayer.Two)]
        public void EndGameScore_TieBreaker_WinnerShouldBePlayerWhoTriggeredEndGame(MancalaPlayer triggerAndWinnerPlayer)
        {
            // arrange
            var game = new Mancala(
                23, new int[] { 0, 0, 0, 0, 0, 1 },
                23, new int[] { 0, 0, 0, 0, 0, 1 },
                triggerAndWinnerPlayer);

            // act
            game.MakeMove(triggerAndWinnerPlayer, 5);

            // assert
            Assert.AreEqual(triggerAndWinnerPlayer, game.Player);
            Assert.IsTrue(game.IsFinished);
        }

        [TestMethod]
        public void MakeMove_ShouldFail_IfAnotherPlayerMakeMoveNotInHisTurn()
        {
            // arrange
            var game = new Mancala(
                23, new int[] { 0, 0, 0, 0, 0, 1 },
                23, new int[] { 0, 0, 0, 0, 0, 1 });

            // act/assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                game.MakeMove(MancalaPlayer.Two, 5);
            });
        }

        [TestMethod]
        public void MakeMove_ShouldFail_IfPitIndexIsOutOfRange()
        {
            // arrange
            var game = new Mancala(
                23, new int[] { 0, 0, 0, 0, 0, 1 },
                23, new int[] { 0, 0, 0, 0, 0, 1 });

            // act/assert
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                game.MakeMove(MancalaPlayer.One, -1);
            });
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                game.MakeMove(MancalaPlayer.One, 6);
            });
        }

        [TestMethod]
        public void MakeMove_ShouldFail_IfPitIsEmpty()
        {
            // arrange
            var game = new Mancala(
                23, new int[] { 0, 0, 0, 0, 0, 1 },
                23, new int[] { 0, 0, 0, 0, 0, 1 });

            // act/assert
            Assert.ThrowsException<InvalidOperationException>(() =>
            {
                game.MakeMove(MancalaPlayer.One, 0);
            });
        }
    }
}
