using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents string splitter which returns words from string one by one.
    /// </summary>
    public class SequentialSplitter
    {
        #region private
        private readonly char[] delimiters = new char [] { '\r', '\n', ' ' };
        #endregion

        /// <summary>
        /// Gets words from text.
        /// </summary>
        /// <param name="text">Text data.</param>
        /// <returns>Word collection.</returns>
        public IEnumerable<string> GetNextWord(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                yield break;
            }

            StringBuilder word = new StringBuilder();

            foreach (char symbol in text)
            {
                if (delimiters.Contains(symbol))
                {
                    if (word.Length == 0)
                    {
                        continue;
                    }

                    yield return word.ToString();
                    word.Clear();
                }
                else
                {
                    word.Append(symbol);
                }
            }

            if (word.Length > 0)
            {
                yield return word.ToString();
            }
        }
    }
}
