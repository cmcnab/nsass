namespace NSass.Compiler
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Environment.Exit(new NSass.Shell.Console().Run(args));
        }
    }
}
