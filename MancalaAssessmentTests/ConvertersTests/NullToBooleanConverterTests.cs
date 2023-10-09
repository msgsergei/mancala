using MancalaWPF.Infrastructure.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MancalaTests.ConvertersTests
{
    [TestClass]
    public class NullToBooleanConverterTests
    {
        [TestMethod]
        public void Convert_ShouldReturnTrueForNonNull()
        {
            // arrange
            var converter = new NullToBooleanConverter();

            // act
            var actualResult = converter.Convert(453, typeof(object), null!, null!);

            // assert
            Assert.IsInstanceOfType(actualResult, typeof(bool));
            Assert.IsTrue((bool)actualResult);
        }

        [TestMethod]
        public void Convert_ShouldReturnFalseForNull()
        {
            // arrange
            var converter = new NullToBooleanConverter();

            // act
            var actualResult = converter.Convert(null!, typeof(object), null!, null!);

            // assert
            Assert.IsInstanceOfType(actualResult, typeof(bool));
            Assert.IsFalse((bool)actualResult);
        }
    }
}
