using System;
using FrequencyCalculationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrequencyCalculationServiceTests
{
    /// <summary>
    /// Tests for <see cref="SequentialSplitter"/> class.
    /// </summary>
    [TestClass]
    public class SequentialSplitterTests
    {
        #region private
        private readonly string testData = @"You can run unit tests in Visual Studio by using third-party test frameworks such as Boost, Google, and NUnit. Use the plug-in for the framework so that Visual Studio's test runner can work with that framework.

Following are the steps to enable third-party test frameworks:
      

Choose   Tools > Extensions and Updates from the menu bar.";

        private readonly string[] expectedSplit;
        #endregion

        public SequentialSplitterTests()
        {
            expectedSplit = testData.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Tests that text properly splitted to words.
        /// </summary>
        [TestMethod]
        public void TestTextIsProperlySplittedToWords()
        {
            var seqSplitter = new SequentialSplitter();

            int i = 0;
            foreach(string word in seqSplitter.GetNextWord(testData))
            {
                Assert.AreEqual(expectedSplit[i], word);
                i++;
            }
        }
    }
}
