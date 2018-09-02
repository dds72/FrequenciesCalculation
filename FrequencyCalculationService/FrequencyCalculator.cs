using System.Collections.Generic;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Tests for <see cref="FrequencyCalculator"/>.
    /// </summary>
    public class FrequencyCalculator : IFrequencyCalculator
    {
        #region private
        private readonly int maxWordLength;
        #endregion

        public FrequencyCalculator(int maxWordLength)
        {
            this.maxWordLength = maxWordLength;
        }

        public IDictionary<string, long> CalculateFrequencies(string data)
        {
            var dictionary = new Dictionary<string, long>();
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

            return dictionary;
        }
    }
}
