﻿using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.
        
        *) Unnecessary '[Flags]' attribute on line 28 - File can either be in a read mode or in write mode
        *) Use StreamReader to read the contents of the file instead File.Open
        *) Use StreamWriter to write the contents to the file instead FileInfo
        *) Unnecessary and incorrect use of const in method Read
        *) Unnecessary separator character array declaration on lines 72 and 101  char[] separator = { '\t' };
        *) Unnecessary and confusing overloading function 'Read' on line 64.
        *) Unnecessary else condition on line 122 will throw exception if columns length is 1
        *) No exception handling/checking in place
        *) Read function on line 93 does not read the complete file
        *) Use string builder to concatenate the string
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _readerStream = File.OpenText(fileName);
            }
            else if (mode == Mode.Write)
            {
                FileInfo fileInfo = new FileInfo(fileName);
                _writerStream = fileInfo.CreateText();
            }
            else
            {
                throw new Exception("Unknown file mode for " + fileName);
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}
