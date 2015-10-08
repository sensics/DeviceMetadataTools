using System;
using System.IO;
using Sensics.SystemUtilities;

namespace Sensics.DeviceMetadataInstaller
{
    public interface ICabFile
    {
        TextReader OpenTextFile(string contained);
    }

    public interface ICabFileFactory
    {
        ICabFile OpenCab(string filename);
    }

    internal sealed class CabFile
    {
        public CabFile(string filename)
        {
            Filename = filename;
        }

        public TextReader OpenTextFile(string contained)
        {
            var sh = new Shell32InstanceWrapper();
            using (var tempFile = DisposableTempPath.GenerateUniqueFilename())
            {

            }
            return new StringReader("bla");
#if false
            var sh = new Shell32.Shell();

            var files = Cab.GetFiles(contained);
            if (files.Count != 1)
            {
                throw new Exception(String.Format("Couldn't find exactly one file named '{0}' in the cab file '{1}!", contained, Filename));
            }
            return files[0].OpenText();
#endif
        }
        private string Filename;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CabFile() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
