using System;
using System.IO;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Reads data by blocks from specified input file.
    /// </summary>
    public class FileDataReader : IDataReader, IDisposable
    {
        #region private
        private readonly FileStream fileStream;

        private readonly IDataReader blockReader;
        #endregion

        /// <summary>
        /// Constructs <see cref="DataReader"/> instance.
        /// </summary>
        /// <param name="filePath">Path to input file.</param>
        /// <param name="minBlockSize">
        /// Minimal size of text block.
        /// Actual block size depends on the first condition reached:
        /// - end of file inside the block;
        /// - following end of word after the block;
        /// - following end of line after the block;
        /// - following end of file after the block;
        /// - the word was not found after block size + maxWordLength is reached.
        /// </param>
        public FileDataReader(
            string filePath,
            int minBlockSize,
            int maxWordLength,
            char[] delimiters)
        {
            fileStream = new FileStream(filePath, FileMode.Open);
            blockReader =
                new StreamBlockReader(
                    minBlockSize,
                    minBlockSize + maxWordLength,
                    delimiters,
                    fileStream);
        }

        /// <summary>
        /// Gets unit of strings from file.
        /// </summary>
        /// <returns>Return text block of appropriate size from file.</returns>
        public string GetBlock()
        {
            return blockReader.GetBlock();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    fileStream.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
