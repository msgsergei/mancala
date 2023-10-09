using MancalaWPF.Infrastructure.Converters;
using MancalaGame;
using MancalaWPF.Infrastructure.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace MancalaTests.ConvertersTests
{
    [TestClass]
    public class MancalaPlayerToVisibilityConverterTests
    {
        [TestMethod]
        [DataRow(null, MancalaPlayer.One, Visibility.Visible)]
        [DataRow(null, MancalaPlayer.Two, Visibility.Visible)]
        [DataRow(MancalaPlayer.One, MancalaPlayer.One, Visibility.Visible)]
        [DataRow(MancalaPlayer.One, MancalaPlayer.Two, Visibility.Collapsed)]
        [DataRow(MancalaPlayer.Two, MancalaPlayer.One, Visibility.Collapsed)]
        [DataRow(MancalaPlayer.Two, MancalaPlayer.Two, Visibility.Visible)]
        public void Convert_ShouldReturnValidBooleanValueDependsOnConfiguration(
            MancalaPlayer? converterPlayer,
            MancalaPlayer bindingObject,
            Visibility expectedResult)
        {
            // arrange
            var converter = new MancalaPlayerToVisibilityConverter
            {
                Player = converterPlayer
            };

            // act
            var actualResult = converter.Convert(bindingObject, typeof(object), null!, null!);

            // assert
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(expectedResult, (Visibility)actualResult);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
