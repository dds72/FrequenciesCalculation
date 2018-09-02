using System.Collections.Generic;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Tests for <see cref="FrequencyCalculatorWithUpdater"/>.
    /// </summary>
    public class FrequencyCalculatorWithUpdater : IFrequencyCalculatorWithUpdater
    {
        #region private
        private readonly int maxWordLength;
        #endregion

        public FrequencyCalculatorWithUpdater(int maxWordLength)
        {
            this.maxWordLength = maxWordLength;
        }

        public void CalculateFrequencies(string data, IDictionary<string, long> dictionary)
        {
            var splitter = new SequentialSplitter();

            foreach (string word in splitter.GetNextWord(data))
            {
                if (word.Length > maxWordLength)
                {
                    // TODO: Log warning.
                    continue;
                }

                if (dictionary.ContainsKey(word))
                {
                    dictionary[word]++;
                }
                else
                {
                    dictionary[word] = 1;
                }
            }
        }
    }
}
