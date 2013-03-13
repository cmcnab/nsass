namespace NSass.Tests.Script
{
    using NSass.Script;
    using Xunit;

    public class TestParser
    {
        [Fact]
        public void Something()
        {
            // Arrange
            var parser = new Parser();
            var input = new Token[]
            {
                Tokens.Symbol("#main"),
                Tokens.Symbol("p"),
                Tokens.LCurly(),
                Tokens.Symbol("color"),
                Tokens.Colon(),
                Tokens.Symbol("#00ff00"),
                Tokens.SemiColon(),
                Tokens.EndInterpolation()
            };

            // Act
            var ast = parser.Parse(input);

            // TODO: Assert
        }
    }
}
