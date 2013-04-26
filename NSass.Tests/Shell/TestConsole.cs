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

        //[Fact]
        //public void JustInFileSpecifiedOutFileHasSameNameCssExtension()
        //{
        //    // Arrange
        //    var fs = new Mock<IFileSystem>();
        //    var engine = new Engine(fs.Object);
        //    const string InputFileName = @"test.scss";
        //    const string ExpectedOutputFileName = @"test.css";
        //    SetupDummyStreams(fs, InputFileName, ExpectedOutputFileName);

        //    // Act
        //    var actualOutputFileName = engine.CompileFile(InputFileName);

        //    // Assert
        //    Assert.Equal(ExpectedOutputFileName, actualOutputFileName);
        //}

        //[Fact]
        //public void JustInPathSpecifiedOutPathHasSameNameCssExtension()
        //{
        //    // Arrange
        //    var fs = new Mock<IFileSystem>();
        //    var engine = new Engine(fs.Object);
        //    const string InputFileName = @"..\foo\test.scss";
        //    const string ExpectedOutputFileName = @"..\foo\test.css";
        //    SetupDummyStreams(fs, InputFileName, ExpectedOutputFileName);

        //    // Act
        //    var actualOutputFileName = engine.CompileFile(InputFileName);

        //    // Assert
        //    Assert.Equal(ExpectedOutputFileName, actualOutputFileName);
        //}

        //[Fact]
        //public void OutFileFileSpecifiedEmptyActualOutFileIsCorrect()
        //{
        //    // Arrange
        //    var fs = new Mock<IFileSystem>();
        //    var engine = new Engine(fs.Object);
        //    const string InputFileName = @"test.scss";
        //    const string ExpectedOutputFileName = @"test.css";
        //    SetupDummyStreams(fs, InputFileName, ExpectedOutputFileName);

        //    // Act
        //    var actualOutputFileName = engine.CompileFile(InputFileName, string.Empty);

        //    // Assert
        //    Assert.Equal(ExpectedOutputFileName, actualOutputFileName);
        //}
    }
}
