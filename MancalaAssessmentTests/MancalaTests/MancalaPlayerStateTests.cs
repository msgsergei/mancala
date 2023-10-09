using MancalaGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MancalaTests.MancalaTests
{
    [TestClass]
    public class MancalaPlayerStateTests
    {
        private MancalaPlayerState BuildMancalaPlayerState(
            int? store = null,
            int[]? pits = null,
            List<MancalaPlayerStateUpdate>? stateTracker = null)
        {
            var state = new MancalaPlayerState(
                store ?? 44,
                pits ?? new[] { 1, 2, 3, 4, 5, 6 });
            state.StateUpdateHandler = (x) =>
            {
                if (stateTracker is not null)
                {
                    stateTracker.Add(x);
                }
            };
            return state;
        }

        [TestMethod]
        public void Constructor_ShouldInitValidState()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Assert
            Assert.AreEqual(44, state.Store);
            Assert.AreEqual(6, state.Pits.Count);
            Assert.AreEqual(1, state.Pits[0]);
            Assert.AreEqual(2, state.Pits[1]);
            Assert.AreEqual(3, state.Pits[2]);
            Assert.AreEqual(4, state.Pits[3]);
            Assert.AreEqual(5, state.Pits[4]);
            Assert.AreEqual(6, state.Pits[5]);
        }

        [TestMethod]
        public void Reset_ShouldResetToInitialState()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Acts
            state.MoveAllStonesFromPitsIntoStore();
            state.Reset();

            // Assert
            Assert.AreEqual(44, state.Store);
            Assert.AreEqual(6, state.Pits.Count);
            Assert.AreEqual(1, state.Pits[0]);
            Assert.AreEqual(2, state.Pits[1]);
            Assert.AreEqual(3, state.Pits[2]);
            Assert.AreEqual(4, state.Pits[3]);
            Assert.AreEqual(5, state.Pits[4]);
            Assert.AreEqual(6, state.Pits[5]);
        }

        [TestMethod]
        public void NumberOfStonesInPit_ShouldReturnValidValue()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Assert
            Assert.AreEqual(1, state.NumberOfStonesInPit(0));
            Assert.AreEqual(2, state.NumberOfStonesInPit(1));
            Assert.AreEqual(3, state.NumberOfStonesInPit(2));
            Assert.AreEqual(4, state.NumberOfStonesInPit(3));
            Assert.AreEqual(5, state.NumberOfStonesInPit(4));
            Assert.AreEqual(6, state.NumberOfStonesInPit(5));
        }

        [TestMethod]
        public void TakeStonesFromPit_ShouldDoNothingIfZeroStonesInPit()
        {
            // Arrange
            var state = BuildMancalaPlayerState(pits: new[] { 0, 2, 3, 4, 5, 6 });

            // act
            var stonesFromPit = state.TakeStonesFromPit(0);

            // assert
            Assert.AreEqual(0, stonesFromPit);
            Assert.AreEqual(0, state.Pits[0]);
        }

        [TestMethod]
        public void TakeStonesFromPit_ShouldGetValidValueAndRemoveStonesFromPit()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Act
            var stonesFromPit = state.TakeStonesFromPit(4);

            // Assert
            Assert.AreEqual(5, stonesFromPit);
            Assert.AreEqual(44, state.Store);
            Assert.AreEqual(1, state.Pits[0]);
            Assert.AreEqual(2, state.Pits[1]);
            Assert.AreEqual(3, state.Pits[2]);
            Assert.AreEqual(4, state.Pits[3]);
            Assert.AreEqual(0, state.Pits[4]); // Removed
            Assert.AreEqual(6, state.Pits[5]);
        }

        [TestMethod]
        public void PutOneStoneIntoPit_ShouldAddOneStoneToSelectedPit()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Act
            state.PutOneStoneIntoPit(4);

            // Assert
            Assert.AreEqual(44, state.Store);
            Assert.AreEqual(1, state.Pits[0]);
            Assert.AreEqual(2, state.Pits[1]);
            Assert.AreEqual(3, state.Pits[2]);
            Assert.AreEqual(4, state.Pits[3]);
            Assert.AreEqual(6, state.Pits[4]); // added 1 stone
            Assert.AreEqual(6, state.Pits[5]);
        }

        [TestMethod]
        public void PutStonesIntoStore_ShouldAddStonesIntoStore()
        {
            // Arrange
            var state = BuildMancalaPlayerState();

            // Act
            state.PutStonesIntoStore(100);

            // Assert
            Assert.AreEqual(144, state.Store); // added 100 into store
            Assert.AreEqual(1, state.Pits[0]);
            Assert.AreEqual(2, state.Pits[1]);
            Assert.AreEqual(3, state.Pits[2]);
            Assert.AreEqual(4, state.Pits[3]);
            Assert.AreEqual(5, state.Pits[4]);
            Assert.AreEqual(6, state.Pits[5]);
        }

        [TestMethod]
        public void MoveAllStonesFromPitsIntoStore_ShouldMoveAllStonesFromPitsToStore()
        {
            // Arrange
            var state = BuildMancalaPlayerState(store: 30, pits: new[] { 8, 0, 5, 0, 2, 3 });

            // Act
            state.MoveAllStonesFromPitsIntoStore();

            // Assert
            Assert.AreEqual(48, state.Store);
            Assert.AreEqual(0, state.Pits[0]);
            Assert.AreEqual(0, state.Pits[1]);
            Assert.AreEqual(0, state.Pits[2]);
            Assert.AreEqual(0, state.Pits[3]);
            Assert.AreEqual(0, state.Pits[4]);
            Assert.AreEqual(0, state.Pits[5]);
        }

        [TestMethod]
        public void MoveAllStonesFromPitsIntoStore_ShouldGenerateValidStateUpdate()
        {
            // Arrange
            var stateUpdates = new List<MancalaPlayerStateUpdate>();
            var state = BuildMancalaPlayerState(store: 30, pits: new[] { 8, 0, 5, 0, 2, 3 }, stateTracker: stateUpdates);

            // Act
            state.MoveAllStonesFromPitsIntoStore();

            // Assert
            Assert.AreEqual(5, stateUpdates.Count);
            Assert.AreEqual(0, stateUpdates[0].PitIndex);
            Assert.AreEqual(-8, stateUpdates[0].Change);
            Assert.AreEqual(2, stateUpdates[1].PitIndex);
            Assert.AreEqual(-5, stateUpdates[1].Change);
            Assert.AreEqual(4, stateUpdates[2].PitIndex);
            Assert.AreEqual(-2, stateUpdates[2].Change);
            Assert.AreEqual(5, stateUpdates[3].PitIndex);
            Assert.AreEqual(-3, stateUpdates[3].Change);
            Assert.AreEqual(null, stateUpdates[4].PitIndex);
            Assert.AreEqual(18, stateUpdates[4].Change);
        }

        [TestMethod]
        public void PutStonesIntoStore_ShouldGenerateValidStateUpdate()
        {
            // Arrange
            var stateUpdates = new List<MancalaPlayerStateUpdate>();
            var state = BuildMancalaPlayerState(store: 30, pits: new[] { 8, 0, 5, 0, 2, 3 }, stateTracker: stateUpdates);

            // Act
            state.PutStonesIntoStore(22);

            // Assert
            Assert.AreEqual(1, stateUpdates.Count);
            Assert.AreEqual(null, stateUpdates[0].PitIndex);
            Assert.AreEqual(22, stateUpdates[0].Change);
        }

        [TestMethod]
        public void TakeStonesFromPit_ShouldGenerateValidStateUpdate()
        {
            // Arrange
            var stateUpdates = new List<MancalaPlayerStateUpdate>();
            var state = BuildMancalaPlayerState(stateTracker: stateUpdates);

            // Act
            state.TakeStonesFromPit(4);

            // Assert
            Assert.AreEqual(1, stateUpdates.Count);
            Assert.AreEqual(4, stateUpdates[0].PitIndex);
            Assert.AreEqual(-5, stateUpdates[0].Change);
        }

        [TestMethod]
        public void PutOneStoneIntoPit_ShouldGenerateValidStateUpdate()
        {
            // Arrange
            var stateUpdates = new List<MancalaPlayerStateUpdate>();
            var state = BuildMancalaPlayerState(stateTracker: stateUpdates);

            // Act
            state.PutOneStoneIntoPit(4);

            // Assert
            Assert.AreEqual(1, stateUpdates.Count);
            Assert.AreEqual(4, stateUpdates[0].PitIndex);
            Assert.AreEqual(1, stateUpdates[0].Change);
        }
    }
}
