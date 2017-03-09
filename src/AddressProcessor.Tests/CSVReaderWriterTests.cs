using System;
using AddressProcessing.CSV;
using NUnit.Framework;
using System.IO;

namespace AddressProcessing.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private const string TestInputFile = @"test_data\contacts.csv";
        private const string TestOutputFile = @"test_data\contacts-out.csv";

        [Test]
        public void Open_Method_Should_throw_Exception()
        {
            var csvReaderWriter = new CSVReaderWriter();

           Assert.Throws<Exception>(() =>csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestInputFile), (CSVReaderWriter.Mode)3));

        }

        [Test]
        public void Open_Method_Should_throw_IOException()
        {
            var csvReaderWriter = new CSVReaderWriter();

            Assert.Throws<IOException>(() => csvReaderWriter.Open(TestInputFile, CSVReaderWriter.Mode.Read));

        }

        [Test]
        public void Open_Method_to_read_Should_not_throw_Exception()
        {
            var csvReaderWriter = new CSVReaderWriter();

            Assert.DoesNotThrow(() => csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestInputFile), CSVReaderWriter.Mode.Read));

            csvReaderWriter.Close();
        }

        [Test]
        public void Open_Method_to_write_Should_not_throw_Exception()
        {
            var csvReaderWriter = new CSVReaderWriter();

            Assert.DoesNotThrow(() => csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile), CSVReaderWriter.Mode.Write));

            csvReaderWriter.Close();
        }

        [Test]
        public void Read_Method_Should_read_first_line_of_file()
        {
            var csvReaderWriter = new CSVReaderWriter();

            csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestInputFile), CSVReaderWriter.Mode.Read);

            string column1, column2;
            csvReaderWriter.Read(out column1, out column2);

            csvReaderWriter.Close();

            Assert.IsTrue(string.Equals(column1, "Shelby Macias"));
            Assert.IsTrue(string.Equals(column2, "3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"));
        }

        [Test]
        public void Read_Method_Should_return_false_with_null_ref_input_string_parameters()
        {
            var csvReaderWriter = new CSVReaderWriter();
            string column1, column2;
            var result = csvReaderWriter.Read(out column1, out column2);
            Assert.False(result);
            Assert.Null(column1);
            Assert.Null(column2);
        }

        [Test]
        public void Read_Method_Should_read_till_end_of_file()
        {
            var csvReaderWriter = new CSVReaderWriter();

            csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestInputFile), CSVReaderWriter.Mode.Read);

            string column1, column2;
            int counter = 0;

            while (csvReaderWriter.Read(out column1, out column2))
            {
                counter++;
            }

            csvReaderWriter.Close();

            Assert.That(counter, Is.EqualTo(229));
        }

        [Test]
        public void Write_Method_Should_not_throw_Exception_when_empty_string_array_is_passed()
        {
            var csvReaderWriter = new CSVReaderWriter();

            Assert.DoesNotThrow(() => csvReaderWriter.Write(new string[] {}));

            csvReaderWriter.Close();
        }

        [Test]
        public void Write_Method_Should_not_throw_Exception_when_non_empty_string_array_is_passed()
        {
            var csvReaderWriter = new CSVReaderWriter();

            Assert.DoesNotThrow(() => csvReaderWriter.Write(new string[] { "test", "data" }));

            csvReaderWriter.Close();
        }

        [Test]
        public void Write_Method_Should_write_to_the_file()
        {
            var csvReaderWriter = new CSVReaderWriter();

            csvReaderWriter.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile), CSVReaderWriter.Mode.Write);
            
            csvReaderWriter.Write(new string[] {"This", "should", "be", "written", "to", "the", "file"});

            csvReaderWriter.Close();

            Assert.True(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile)));
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile));
        }

        [Test]
        public void Read_and_Write_Methods_Should_read_and_write_file()
        {
            var reader = new CSVReaderWriter();
            var writer = new CSVReaderWriter();
            int counter = 0;

            reader.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestInputFile), CSVReaderWriter.Mode.Read);
            writer.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile), CSVReaderWriter.Mode.Write);

            string column1, column2;
            while (reader.Read(out column1, out column2))
            {
                writer.Write(new string[] { column1, column2 });
                counter++;
            }

            reader.Close();
            writer.Close();

            Assert.That(counter, Is.EqualTo(229));
            Assert.True(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile)));
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestOutputFile));

        }
    }
}