using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NSass.Shell;
using Xunit;

namespace NSass.Tests.Shell
{
    public class TestConsole
    {
        [Fact]
        public void Something()
        {
            // Arrange
            var io = new Mock<IConsoleIO>();
            var fs = new Mock<ISassCompiler>();
            var console = new Console(io.Object, fs.Object);

            // Act

            // Assert
        }
    }
}
