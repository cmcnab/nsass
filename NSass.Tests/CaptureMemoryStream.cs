namespace NSass.Tests
{
    using System.IO;

    public class CaptureMemoryStream : MemoryStream
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
