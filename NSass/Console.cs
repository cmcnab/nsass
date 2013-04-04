namespace NSass
{
    using System;

    public class Console
    {
        private readonly ISassCompiler engine;

        public Console()
        {
            this.engine = new Engine(new FileSystem.FileSystem());
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
                System.Console.Error.WriteLine(ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                // This is something unexpected so we'll print the whole stack trace.
                System.Console.Error.WriteLine(ex.ToString());
                return -2;
            }
        }
    }
}
