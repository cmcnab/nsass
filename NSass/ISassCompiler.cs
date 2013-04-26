﻿namespace NSass
{
    using System.IO;

    public interface ISassCompiler
    {
        string Compile(string input);

        TextWriter Compile(TextReader input, TextWriter output);
    }
}
