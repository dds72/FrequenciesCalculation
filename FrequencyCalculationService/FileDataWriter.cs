using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Saves dictionary to file on disk.
    /// </summary>
    public class FileDataWriter : IDataWriter
    {
        #region private
        private readonly string filePath;
        #endregion

        /// <summary>
        /// Constructs <see cref="FileDataWriter"/>.
        /// </summary>
        /// <param name="filePath">Path to file where where data should be saved.</param>
        public FileDataWriter(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Saves dictionary to file on disk in Windows-1251 codepage.
        /// If file exists it will be overwritten.
        /// </summary>
        /// <param name="frequencyDictionary">Dictionary with data to save in file.</param>
        public void SaveDictionary(IDictionary<string, long> frequencyDictionary)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                Encoding encoding = Encoding.GetEncoding("windows-1251");

                foreach (KeyValuePair<string, long> frequencyPair in frequencyDictionary)
                {
                    byte[] record = encoding.GetBytes($"{frequencyPair.Key},{frequencyPair.Value}\n");
                    fileStream.Write(record, 0, record.Length);
                }

                fileStream.Flush();
            }
        }
    }
}
