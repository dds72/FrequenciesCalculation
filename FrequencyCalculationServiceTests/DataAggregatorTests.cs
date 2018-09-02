using FrequencyCalculationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FrequencyCalculationServiceTests
{
    /// <summary>
    /// Tests for <see cref="SequentialSplitter"/> class.
    /// </summary>
    [TestClass]
    public class DataAggregatorTests
    {
        #region private
        private const int ThreadsCount = 10;

        private const int MergeOperationsCount = 10;

        private readonly Dictionary<string, long> firstDictionary =
            new Dictionary<string, long> {
                { "microsoft", 15 },
                { "twitter", 4 },
                { "hoolie", 77 },
                { "you", 9 },
                { "are", 11 },
                { "king", 9 },
            };

        private readonly Dictionary<string, long> secondDictionary =
            new Dictionary<string, long> {
                { "you", 4 },
                { "are", 5 },
                { "king", 9 },
                { "facebook", 7 },
                { "twitter", 4 },
                { "hoolie", 77 },
                { "tree", 34 },
            };

        private readonly Dictionary<string, long> expectedData =
            new Dictionary<string, long> {
                { "are", 16 },
                { "facebook", 7 },
                { "hoolie", 154 },
                { "king", 18 },
                { "microsoft", 15 },
                { "tree", 34 },
                { "twitter", 8 },
                { "you", 13 },
            };

        private readonly string[] multiThreadedStrings =
            new string[] {
                "first",
                "second",
                "third",
                "fourth",
                "fifth",
                "sixth",
                "seventh",
                "eighth",
                "ninth",
                "tenth",
            };

        private readonly Dictionary<string, long>[] multiThreadedDictionaries;

        private readonly Dictionary<string, long> expectedMultiThreadedResult =
            new Dictionary<string, long>();
        #endregion

        public DataAggregatorTests()
        {
            multiThreadedDictionaries = new Dictionary<string, long>[ThreadsCount];
            for (int i = 0; i < ThreadsCount; i++)
            {
                multiThreadedDictionaries[i] = new Dictionary<string, long>();
                foreach (string element in multiThreadedStrings)
                {
                    multiThreadedDictionaries[i][element] = 1;
                }
            }

            foreach (string element in multiThreadedStrings.OrderBy(s => s))
            {
                expectedMultiThreadedResult[element] = MergeOperationsCount * ThreadsCount;
            }
        }

        /// <summary>
        /// Tests that dictionaries properly merged and result dictionary is ordered.
        /// </summary>
        [TestMethod]
        public void TestDictionariesAreProperlyMerged()
        {
            var dataAggregator = new DataAggregator();

            dataAggregator.MergeData(firstDictionary);
            dataAggregator.MergeData(secondDictionary);

            IDictionary<string, long> actualData = dataAggregator.GetData();

            TestHelpers.AssertEqualsDictionaries(expectedData, actualData);
        }

        /// <summary>
        /// Tests that merging dictionaries from different threads works properly.
        /// </summary>
        public void TestMultithreadedMergeWorksProperly()
        {
            var dataAggregator = new DataAggregator();
            Thread[] threads = new Thread[ThreadsCount];

            for (int i = 0; i < threads.Length; i++)
            {
                int threadNumber = i;
                threads[i] = new Thread(() => {
                    for (int j = 0; j < MergeOperationsCount; j++)
                    {
                        dataAggregator.MergeData(multiThreadedDictionaries[threadNumber]);
                    }
                });
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            IDictionary<string, long> actualData = dataAggregator.GetData();

            TestHelpers.AssertEqualsDictionaries(expectedMultiThreadedResult, actualData);
        }
    }
}
