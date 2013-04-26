namespace NSass.Shell
{
    using System;

    public class Console
    {
        private readonly IConsoleIO io;
        private readonly ISassCompiler engine;

        public Console()
            : this(new ConsoleIO(), new Engine(new FileSystem.FileSystem()))
        {
        }

        public Console(IConsoleIO io, ISassCompiler engine)
        {
            this.io = io;
            this.engine = engine;
        }

        public int Run(string[] args)
        {
            try
            {
                // TODO: implement help/usage and other arguments
                var inputFile = args.Length > 0 ? args[0] : string.Empty;
                var outputFile = args.Length > 1 ? args[1] : string.Empty;

                this.engine.CompileFile(inputFile, outputFile);
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
        }
    }
}
