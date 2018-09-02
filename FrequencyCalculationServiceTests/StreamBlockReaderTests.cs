using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

using FrequencyCalculationService;
using System.Collections.Generic;

namespace FrequencyCalculationServiceTests
{
    /// <summary>
    /// Tests for <see cref="StreamBlockReader"/> class.
    /// </summary>
    [TestClass]
    public class StreamBlockReaderTests
    {
        #region private
        private const int minBlockSize = 500;
        private const int maxWordLength = 20;
        private const int blocksCount = 3;

        private readonly string[] blocks = new string[blocksCount];
        private readonly char[] delimiters = new char[] { '\r', '\n', ' ' };
        private Stream fullTextStream;
        #endregion

        public StreamBlockReaderTests()
        {
            fullTextStream = TestHelpers.GetStreamFromEmbeddedResource("TextToBlocks.txt");
            for (int i = 0; i < 3; i++)
            {
                blocks[i] = TestHelpers.ReadFromEmbeddedResource($"Block{i + 1}.txt");
            }
        }

        /// <summary>
        /// Tests that stream readed by blocks properly.
        /// </summary>
        [TestMethod]
        public void TestStreamReadedByBlocksProperly()
        {
            var reader = 
                new StreamBlockReader(
                    minBlockSize,
                    minBlockSize + maxWordLength,
                    delimiters,
                    fullTextStream);

            List<string> actualBlocks = new List<string>();

            while (true)
            {
                string block = reader.GetBlock();
                if (block == null)
                {
                    break;
                }
                actualBlocks.Add(block);
            }

            Assert.AreEqual(blocks.Length, actualBlocks.Count);
            for (int i = 0; i < blocks.Length; i++)
            {
                Assert.AreEqual(blocks[i].Length, actualBlocks[i].Length);
                Assert.AreEqual(blocks[i], actualBlocks[i]);
            }
        }
    }
}
