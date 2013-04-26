namespace NSass.Shell
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using NSass.FileSystem;

    public class Console
    {
        private const string CssFileExtension = ".css";

        private readonly IConsoleIO io;
        private readonly IFileSystem fileSystem;
        private readonly ISassCompiler engine;

        [ExcludeFromCodeCoverage]
        public Console()
            : this(new ConsoleIO(), new FileSystem(), new Engine())
        {
        }

        public Console(IConsoleIO io, IFileSystem fileSystem, ISassCompiler engine)
        {
            this.io = io;
            this.fileSystem = fileSystem;
            this.engine = engine;
        }

        public int Run(string[] args)
        {
            TextReader input = null;
            TextWriter output = null;

            try
            {
                // TODO: implement help/usage and other arguments
                input = this.GetInput(args.Length > 0 ? args[0] : string.Empty);
                output = this.GetOutput(args.Length > 1 ? args[1] : string.Empty);

                this.engine.Compile(input, output);
                return 0;
            }
            catch (SassException ex)
            {
                // "Normal" exceptions are wrapped in a SassException.
                this.io.Error.WriteLine(ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                // This is something unexpected so we'll print the whole stack trace.
                this.io.Error.WriteLine(ex.ToString());
                return -2;
            }
            finally
            {
                if (input != null)
                {
                    input.Dispose();
                }

                if (output != null)
                {
                    output.Dispose();
                }
            }
        }

        private TextReader GetInput(string inputFilePath)
        {
            return string.IsNullOrEmpty(inputFilePath)
                ? this.io.In
                : new StreamReader(this.fileSystem.OpenFile(inputFilePath, FileMode.Open, FileAccess.Read));
        }

        private TextWriter GetOutput(string outputFilePath)
        {
            return string.IsNullOrEmpty(outputFilePath)
                ? this.io.Out
                : new StreamWriter(this.fileSystem.OpenFile(outputFilePath, FileMode.Create, FileAccess.Write));
        }

        //private static string GetOutputFilePath(string inputFilePath)
        //{
        //    return Path.ChangeExtension(inputFilePath, CssFileExtension);
        //}
    }
}
