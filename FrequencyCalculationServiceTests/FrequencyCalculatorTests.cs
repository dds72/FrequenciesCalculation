using System;
using System.Collections.Generic;
using FrequencyCalculationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrequencyCalculationServiceTests
{
    /// <summary>
    /// Tests for <see cref="FrequencyCalculator"/> class.
    /// </summary>
    [TestClass]
    public class FrequencyCalculatorTests
    {
        #region private
        private readonly string testData = @"you are king
you are king you are king you are king you are king you are king you are king you are king
you are king

are are ExtensionsandUpdatesfromthemenubar      


";

        private readonly Dictionary<string, long> expectedFrequencies = new Dictionary<string, long> {
            { "you", 9 },
            { "are", 11 },
            { "king", 9 }
        };

        private int maxWordLength = 20;
        #endregion

        /// <summary>
        /// Tests that words frequencies properly calculated.
        /// </summary>
        [TestMethod]
        public void TestFrequenciesOfWordsProperlyCalculated()
        {
            var calculator = new FrequencyCalculator(maxWordLength);
            IDictionary<string, long> actualFrequencies = calculator.CalculateFrequencies(testData);

            foreach (KeyValuePair<string, long> frequencyPair in actualFrequencies)
            {
                Assert.IsTrue(expectedFrequencies.ContainsKey(frequencyPair.Key));
                Assert.AreEqual(expectedFrequencies[frequencyPair.Key], frequencyPair.Value);
            }
        }
    }
}
