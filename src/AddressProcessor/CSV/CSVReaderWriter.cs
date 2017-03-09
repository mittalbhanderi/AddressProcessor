using System;
using System.IO;
using System.Text;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        //'[Flags]' attribute removed - File can either be in a read mode or in write mode
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            try
            {
                if (mode == Mode.Read)
                {
                    //StreamReader is lightweight and more efficient compared to File.OpenText
                    _readerStream = new StreamReader(fileName, true);
                }
                else if (mode == Mode.Write)
                {
                    //StreamWriter is the direct instance of CreateText() method without initializing FileInfo class 
                    _writerStream = new StreamWriter(fileName);
                }
                else
                {
                    throw new Exception("Unknown file mode for " + fileName);
                }
            }
            // catch and throw IOException in case the file could be found.
            catch (IOException ioex)
            {
                throw new IOException($"Error opening/locating the file {ioex.Message}");
            }
        }

        public void Write(params string[] columns)
        {
            // Comment the if condition below to write empty string line to the file
            if (columns.Length > 0)
            {
                // string variable replaced with Strigbuilder for better performance
                StringBuilder outPut = new StringBuilder();

                for (int i = 0; i < columns.Length; i++)
                {
                    outPut.Append(columns[i]);
                    if ((columns.Length - 1) != i)
                    {
                        outPut.Append("\t");
                    }
                }

                WriteLine(outPut.ToString());
            }
        }

        public bool Read(out string column1, out string column2)
        {
            string line = ReadLine();

            if (line == null)
            {
                // If there is a blank like in between ignore it and read the next line
                return ReadNextLine(out column1, out column2);
            }
            // We just need a char '\t' rather a char array..
            string[] columns = line.Split('\t');

            if (columns.Length < 2)
            {
                // If there is a line with just one column (tab group) then ignore it and read the next line
                return ReadNextLine(out column1, out column2);
            }

            // line read is valid - set values and return
            // additional conditions required if we want the values to be checked for null or empty!
            column1 = columns[0];
            column2 = columns[1];

            return true;
        }

        private void WriteLine(string line)
        {
            // make sure that _writeStream is not null
            _writerStream?.WriteLine(line);
        }

        private string ReadLine()
        {
            // make sure that _readStream is not null
            return _readerStream?.ReadLine();
        }

        private bool ReadNextLine(out string column1, out string column2)
        {
            // Check if it is end of file
            if (_readerStream != null && !_readerStream.EndOfStream)
            {
                // continue reading
                return Read(out column1, out column2);
            }
            // We have reached end of file 
            column1 = null;
            column2 = null;
            return false;
        }

        public void Close()
        {
            // C# 6.0 - Usage of null conditional operator
            _writerStream?.Close();
            _readerStream?.Close();
        }
    }
}
