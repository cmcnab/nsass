﻿namespace NSass.Parse.Expressions
{
    using System.Collections.Generic;

    public class Rule : Statement
    {
        private readonly Selectors selectors;
        private readonly Body body;

        public Rule(Selectors selectors, Body body)
        {
            this.selectors = selectors;
            this.body = body;
        }

        public Selectors Selectors
        {
            get { return this.selectors; }
        }

        public Body Body
        {
            get { return this.body; }
        }
    }
}
