namespace NSass.Tests
{
    using System.IO;
    using System.Text;
    using Xunit;

    public class TestEngine
    {
        [Fact]
        public void CompileFileSameResultsAsCompileString()
        {
            // Arrange
            var engine = new Engine();
            const string InputText =
@"#navbar {
  width: 80%;
  height: 23px;
}";

            using (var inputStream = new StreamReader(new MemoryStream(UTF8Encoding.Default.GetBytes(InputText))))
            {
                var captureOut = new CaptureMemoryStream();
                var outputStream = new StreamWriter(captureOut);

                // Act
                var compileStringResults = engine.Compile(InputText);
                engine.Compile(inputStream, outputStream);
                outputStream.Dispose();

                // Assert
                Assert.Equal(compileStringResults, captureOut.CapturedString);
            }
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
