using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FrequencyCalculationServiceTests
{
    public class TestHelpers
    {
        #region private
        private const string resorcesPrefix = "FrequencyCalculationServiceTests.TestData";
        #endregion

        /// <summary>
        /// Gets stream with embedded resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>Stream with embedded resource.</returns>
        public static Stream GetStreamFromEmbeddedResource(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"{resorcesPrefix}.{resourceName}");
        }

        /// <summary>
        /// Gets string from embedded resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>String with embedded resource.</returns>
        public static string ReadFromEmbeddedResource(string resourceName)
        {
            using (GetStreamFromEmbeddedResource(resourceName))
            using (StreamReader reader = new StreamReader(
                GetStreamFromEmbeddedResource(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Asserts that two dictionaries have the same key value pairs with equal order.
        /// </summary>
        /// <typeparam name="K">Type of dictioanry key.</typeparam>
        /// <typeparam name="V">Type of dictionary value.</typeparam>
        /// <param name="expectedDictionary">Expected dictionary.</param>
        /// <param name="actualDictionary">Actual dictionary.</param>
        public static void AssertEqualsDictionaries<K, V>(
            IDictionary<K, V> expectedDictionary,
            IDictionary<K, V> actualDictionary)
        {
            Assert.AreEqual(expectedDictionary.Count, actualDictionary.Count);

            KeyValuePair<K, V>[] actualArray = actualDictionary.ToArray();
            KeyValuePair<K, V>[] expectedArray = expectedDictionary.ToArray();

            for (int i = 0; i < actualArray.Length; i++)
            {
                Assert.AreEqual(expectedArray[i].Key, actualArray[i].Key);
                Assert.AreEqual(expectedArray[i].Value, actualArray[i].Value);
            }
        }
    }
}
