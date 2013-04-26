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
            fs.Setup(f => f.OpenFile(InputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());

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
            fs.Setup(f => f.OpenFile(InputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());
            const string OutputFileName = "output.scss";
            fs.Setup(f => f.OpenFile(OutputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());

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

            TextReader stdIn = new StreamReader(new MemoryStream());
            io.Setup(i => i.In).Returns(stdIn);

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
            fs.Setup(f => f.OpenFile(InputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());
            TextWriter stdOut = new StreamWriter(new MemoryStream());
            io.Setup(i => i.Out).Returns(stdOut);

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
            fs.Setup(f => f.OpenFile(InputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());
            const string OutputFileName = "output.scss";
            fs.Setup(f => f.OpenFile(OutputFileName, It.IsAny<FileMode>(), It.IsAny<FileAccess>())).Returns(new MemoryStream());

            const string ParseExceptionMessage = "message";
            engine.Setup(e => e.Compile(It.IsAny<TextReader>(), It.IsAny<TextWriter>())).Throws(new SassException(ParseExceptionMessage));

            var errorCapture = new CaptureMemoryStream();
            var stdError = new StreamWriter(errorCapture);
            io.Setup(i => i.Error).Returns(stdError);

            // Act
            console.Run(Params.Get(InputFileName, OutputFileName));
            stdError.Dispose();

            // Assert
            var expected = ParseExceptionMessage + stdError.NewLine;
            Assert.Equal(expected, errorCapture.CapturedString);
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

            var errorCapture = new CaptureMemoryStream();
            var stdError = new StreamWriter(errorCapture);
            io.Setup(i => i.Error).Returns(stdError);

            // Act
            console.Run(Params.Get(InputFileName));
            stdError.Dispose();

            // Assert
            var expected = inputException.ToString() + stdError.NewLine;
            Assert.Equal(expected, errorCapture.CapturedString);
        }
    }
}
