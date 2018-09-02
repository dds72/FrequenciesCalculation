using System;
using System.IO;
using System.Linq;
using System.Text;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Reads words from stream by blocks 
    /// </summary>
    public class StreamBlockReader : IDataReader
    {
        #region private
        private readonly int minBlockSize;

        private readonly int maxBlockSize;

        private readonly char[] delimiters;

        private Stream stream;
        #endregion

        public StreamBlockReader(
            int minBlockSize,
            int maxBlockSize,
            char[] delimiters,
            Stream stream)
        {
            this.minBlockSize = minBlockSize;
            this.maxBlockSize = maxBlockSize;
            this.delimiters = delimiters;
            this.stream = stream;
        }

        /// <summary>
        /// Gets unit of strings from file.
        /// </summary>
        /// <returns>
        /// Return text block of appropriate size from file.
        /// null, if the end of stream has been reached.
        /// </returns>
        public string GetBlock()
        {
            if (stream.Position == stream.Length)
            {
                return null;
            }

            byte[] buffer = new byte[maxBlockSize];

            int bufferPosition = stream.Read(buffer, 0, minBlockSize);

            while (true)
            {
                int nextValue = stream.ReadByte();
                if (nextValue == -1)
                {
                    break;
                }

                byte nextByte = Convert.ToByte(nextValue);

                buffer[bufferPosition++] = nextByte;

                if (delimiters.Contains((char)nextByte) ||
                    bufferPosition >= maxBlockSize)
                {
                    break;
                }
            }

            Encoding encoding = Encoding.GetEncoding("windows-1251");

            string block = encoding.GetString(buffer, 0, bufferPosition);

            return block;
        }

    }
}
