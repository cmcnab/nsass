namespace NSass.Tests
{
    using System.IO;
    using System.Text;
    using Moq;
    using NSass.FileSystem;
    using Xunit;

    public class TestEngine
    {
        [Fact]
        public void JustInFileSpecifiedOutFileHasSameNameCssExtension()
        {
            // Arrange
            var fs = new Mock<IFileSystem>();
            var engine = new Engine(fs.Object);
            const string InputFileName = @"test.scss";
            const string ExpectedOutputFileName = @"test.css";
            SetupDummyStreams(fs, InputFileName, ExpectedOutputFileName);

            // Act
            var actualOutputFileName = engine.CompileFile(InputFileName);

            // Assert
            Assert.Equal(ExpectedOutputFileName, actualOutputFileName);
        }

        [Fact]
        public void JustInPathSpecifiedOutPathHasSameNameCssExtension()
        {
            // Arrange
            var fs = new Mock<IFileSystem>();
            var engine = new Engine(fs.Object);
            const string InputFileName = @"..\foo\test.scss";
            const string ExpectedOutputFileName = @"..\foo\test.css";
            SetupDummyStreams(fs, InputFileName, ExpectedOutputFileName);

            // Act
            var actualOutputFileName = engine.CompileFile(InputFileName);

            // Assert
            Assert.Equal(ExpectedOutputFileName, actualOutputFileName);
        }

        [Fact]
        public void InputFileIsOpenedForReading()
        {
            // Arrange
            var fs = new Mock<IFileSystem>();
            var engine = new Engine(fs.Object);
            const string InputFileName = @"test.scss";
            const string OutputFileName = @"test.css";
            SetupDummyStreams(fs, InputFileName, OutputFileName);

            // Act
            engine.CompileFile(InputFileName);

            // Assert
            fs.Verify(f => f.OpenFile(InputFileName, FileMode.Open, FileAccess.Read));
        }

        [Fact]
        public void OutputFileIsOpenedForWriting()
        {
            // Arrange
            var fs = new Mock<IFileSystem>();
            var engine = new Engine(fs.Object);
            const string InputFileName = @"test.scss";
            const string OutputFileName = @"test.css";
            SetupDummyStreams(fs, InputFileName, OutputFileName);

            // Act
            engine.CompileFile(InputFileName);

            // Assert
            fs.Verify(f => f.OpenFile(OutputFileName, FileMode.Create, FileAccess.Write));
        }

        [Fact]
        public void CompileFileSameResultsAsCompileString()
        {
            // Arrange
            var fs = new Mock<IFileSystem>();
            var engine = new Engine(fs.Object);

            const string InputFileName = @"test.scss";
            const string OutputFileName = @"test.css";
            const string InputText =
@"#navbar {
  width: 80%;
  height: 23px;
}";

            var outputStream = SetupCaptureStream(fs, InputFileName, OutputFileName, InputText);

            // Act
            var compileStringResults = engine.Compile(InputText);
            engine.CompileFile(InputFileName);

            // Assert
            Assert.Equal(compileStringResults, outputStream.CapturedString);
        }

        private static void SetupDummyStreams(Mock<IFileSystem> fs, string inputFileName, string outputFileName)
        {
            fs.Setup(f => f.OpenFile(inputFileName, FileMode.Open, FileAccess.Read)).Returns(new MemoryStream());
            fs.Setup(f => f.OpenFile(outputFileName, FileMode.Create, FileAccess.Write)).Returns(new MemoryStream());
        }

        private static CaptureMemoryStream SetupCaptureStream(Mock<IFileSystem> fs, string inputFileName, string outputFileName, string inputText)
        {
            var inputStream = new MemoryStream(UTF8Encoding.Default.GetBytes(inputText));
            var outputStream = new CaptureMemoryStream();

            fs.Setup(f => f.OpenFile(inputFileName, FileMode.Open, FileAccess.Read)).Returns(inputStream);
            fs.Setup(f => f.OpenFile(outputFileName, FileMode.Create, FileAccess.Write)).Returns(outputStream);

            return outputStream;
        }

        private class CaptureMemoryStream : MemoryStream
        {
            public string CapturedString { get; private set; }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    this.Seek(0, SeekOrigin.Begin);
                    this.CapturedString = new StreamReader(this).ReadToEnd(); // Already disposing this stream anyway.
                }

                base.Dispose(disposing);
            }
        }
    }
}
