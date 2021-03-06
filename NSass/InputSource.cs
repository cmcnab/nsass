﻿namespace NSass
{
    using System.IO;

    public class InputSource
    {
        public const string StdInFileName = "standard input";

        private readonly string fileName;
        private readonly TextReader reader;

        public InputSource(string fileName, TextReader reader)
        {
            this.fileName = fileName;
            this.reader = reader;
        }

        public string FileName
        {
            get { return this.fileName; }
        }

        public TextReader Reader
        {
            get { return this.reader; }
        }

        public static InputSource FromString(string input)
        {
            return FromStream(new StringReader(input));
        }

        public static InputSource FromStream(TextReader input)
        {
            return new InputSource(StdInFileName, input);
        }
    }
}
