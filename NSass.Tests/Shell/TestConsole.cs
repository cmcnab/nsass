namespace NSass.Tests.Shell
{
    using System.IO;
    using Moq;
    using NSass.FileSystem;
    using NSass.Shell;
    using NSass.Util;
    using Xunit;

    public class TestConsole
    {
        [Fact]
        public void FirstArgumentOpenedAsInputStream()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            const string InputFileName = "input.scss";
            SetDummyFile(fs, InputFileName);

            // Act
            console.Run(Params.Get(InputFileName));

            // Assert
            fs.Verify(f => f.OpenFile(InputFileName, FileMode.Open, FileAccess.Read));
        }

        [Fact]
        public void SecondArgumentOpenedAsOutputStream()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            const string InputFileName = "input.scss";
            SetDummyFile(fs, InputFileName);
            const string OutputFileName = "output.scss";
            SetDummyFile(fs, OutputFileName);

            // Act
            console.Run(Params.Get(InputFileName, OutputFileName));

            // Assert
            fs.Verify(f => f.OpenFile(OutputFileName, FileMode.Create, FileAccess.Write));
        }

        [Fact]
        public void NoFirstArgumentInputOpenedFromStandardIn()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            var stdIn = SetDummyStdIn(io);

            // Act
            console.Run(new string[] { });

            // Assert
            engine.Verify(e => e.Compile(stdIn, It.IsAny<TextWriter>()));
        }

        [Fact]
        public void NoSecondArgumentOutputOpenedAsStandardOut()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            const string InputFileName = "input.scss";
            SetDummyFile(fs, InputFileName);
            var stdOut = SetDummyStdOut(io);

            // Act
            console.Run(Params.Get(InputFileName));

            // Assert
            engine.Verify(e => e.Compile(It.IsAny<TextReader>(), stdOut));
        }

        [Fact]
        public void ParseExceptionMessagePrintedToStdError()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            const string InputFileName = "input.scss";
            SetDummyFile(fs, InputFileName);
            const string OutputFileName = "output.scss";
            SetDummyFile(fs, OutputFileName);

            const string ParseExceptionMessage = "message";
            engine.Setup(e => e.Compile(It.IsAny<TextReader>(), It.IsAny<TextWriter>())).Throws(new SassException(ParseExceptionMessage));

            var err = CaptureStdError(io);

            // Act
            console.Run(Params.Get(InputFileName, OutputFileName));
            err.Item1.Dispose();

            // Assert
            var expected = ParseExceptionMessage + err.Item1.NewLine;
            Assert.Equal(expected, err.Item2.CapturedString);
        }

        [Fact]
        public void UnexpectedExceptionOpeningInputFilePrintedToStdError()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<IFileSystem>();
            var engine = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object, engine.Object);

            const string InputFileName = "input.scss";
            var inputException = new IOException("message");
            fs.Setup(f => f.OpenFile(InputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Throws(inputException);

            var err = CaptureStdError(io);

            // Act
            console.Run(Params.Get(InputFileName));
            err.Item1.Dispose();

            // Assert
            var expected = inputException.ToString() + err.Item1.NewLine;
            Assert.Equal(expected, err.Item2.CapturedString);
        }

        private static void SetDummyFile(Mock<IFileSystem> fs, string fileName)
        {
            fs.Setup(f => f.OpenFile(fileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());
        }

        private static TextReader SetDummyStdIn(Mock<IConsoleIO> io)
        {
            var stdIn = new StreamReader(new MemoryStream());
            io.Setup(i => i.In).Returns(stdIn);
            return stdIn;
        }

        private static TextWriter SetDummyStdOut(Mock<IConsoleIO> io)
        {
            var stdOut = new StreamWriter(new MemoryStream());
            io.Setup(i => i.Out).Returns(stdOut);
            return stdOut;
        }

        private static System.Tuple<StreamWriter, CaptureMemoryStream> CaptureStdError(Mock<IConsoleIO> io)
        {
            var errorCapture = new CaptureMemoryStream();
            var stdError = new StreamWriter(errorCapture);
            io.Setup(i => i.Error).Returns(stdError);
            return System.Tuple.Create(stdError, errorCapture);
        }
    }
}
