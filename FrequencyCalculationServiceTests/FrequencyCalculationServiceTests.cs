using System;
using System.IO;
using FrequencyCalculationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrequencyCalculationServiceTests
{
    /// <summary>
    /// Tests for <see cref="ICalculationService"/> implementations.
    /// </summary>
    [TestClass]
    public class FrequencyCalculationServiceTests
    {
        #region private
        private const int MaxWordLength = 20;
        private const int MinBlockSize = 1024;
        private static readonly char[] delimiters = new char[] { '\r', '\n', ' ' };

        private Stream inputStream;
        #endregion

        [TestInitialize]
        public void Setup()
        {
            inputStream = TestHelpers.GetStreamFromEmbeddedResource("BigText.txt");
        }

        /// <summary>
        /// Tests that service reads data and creates output dictionary.
        /// </summary>
        [TestMethod]
        public void TestTextIsSplittedToWordsInSingleThread()
        {
            IDataWriter dataWriter = new TestDataWriter();
            SingleThreadedFrequencyCalculationService calculationService =
                new SingleThreadedFrequencyCalculationService(
                    new StreamBlockReader(MinBlockSize, MinBlockSize + MaxWordLength, delimiters, inputStream),
                    dataWriter,
                    new DataAggregator(),
                    new FrequencyCalculator(MaxWordLength));
            calculationService.RunCalculation();

            Assert.AreEqual(80985, ((TestDataWriter)dataWriter).Data.Count);
        }

        /// <summary>
        /// Tests that service reads data and creates output dictionary.
        /// </summary>
        [TestMethod]
        public void TestTextIsSplittedToWordsInParallel()
        {
            IDataWriter dataWriter = new TestDataWriter();
            FrequencyCalculationService.FrequencyCalculationService calculationService =
                new FrequencyCalculationService.FrequencyCalculationService(
                    new StreamBlockReader(MinBlockSize, MinBlockSize + MaxWordLength, delimiters, inputStream),
                    dataWriter,
                    new DataAggregator(),
                    new FrequencyCalculator(MaxWordLength));
            calculationService.RunCalculationAsync().Wait();

            Assert.AreEqual(80985, ((TestDataWriter)dataWriter).Data.Count);
        }

        /// <summary>
        /// Tests that service reads data and creates output dictionary.
        /// </summary>
        [TestMethod]
        public void TestTextIsSplittedToWordsInParallelWithOneDictionary()
        {
            IDataWriter dataWriter = new TestDataWriter();
            FrequencyCalculationService.NewFrequencyCalculationService calculationService =
                new NewFrequencyCalculationService(
                    new StreamBlockReader(MinBlockSize, MinBlockSize + MaxWordLength, delimiters, inputStream),
                    dataWriter,
                    new FrequencyCalculatorWithUpdater(MaxWordLength));
            calculationService.RunCalculationAsync().Wait();

            Assert.AreEqual(80985, ((TestDataWriter)dataWriter).Data.Count);
        }
    }
}
