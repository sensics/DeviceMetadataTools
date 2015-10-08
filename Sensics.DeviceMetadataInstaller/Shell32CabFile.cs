using Sensics.SystemUtilities;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sensics.DeviceMetadataInstaller
{
    internal sealed class Shell32CabFile : ICabFile, IDisposable
    {
        public Shell32CabFile(Shell32InstanceWrapper shell, string filename)
        {
            _sh = shell;
            _cab = DisposableTempPath.CopyToUniqueFilename(filename, ".cab");
        }

        public TextReader OpenTextFile(string contained)
        {
            var cabFolder = _sh.NameSpace(_cab.PathName);
            using (var destDir = DisposableTempPath.CreateUniqueDirectory())
            {
                var destFolder = _sh.NameSpace(destDir.PathName);
                var tempFile = Path.Combine(destDir.PathName, Path.GetFileName(contained));
                using (var gotChanged = new ManualResetEventSlim())
                {
                    using (var fsw = new FileSystemWatcher(destDir.PathName))
                    {
                        fsw.Changed += delegate (object sender, FileSystemEventArgs e)
                        {
                            Debug.WriteLine("got changed event {0}", e);
                            gotChanged.Set();
                        };
                        destFolder.CopyHere(cabFolder.ParseName(contained), 0 /*4  do not show file copy dialog */);
                        // Do not proceed until file copy has at least begun.
                        gotChanged.Wait();
                    }
                }
                // OK, file copy has begin, now try loading.
                var text = AttemptToReadAllText(tempFile, TimeSpan.FromSeconds(.5));
#if false
                if (!File.Exists(tempFile))
                {
                    throw new Exception(string.Format("Could not find {0} after supposedly copying it from the cab!", tempFile));
                }
#endif
                return new StringReader(text);
            }
        }

        private static void SleepToRetry()
        {
            // Yes, not ideal - should be waiting on the file itself.
            Thread.Sleep(20);
        }

        private static string AttemptToReadAllText(string filename, TimeSpan timeout)
        {
            bool success = false;
            string ret = "";
            var sw = Stopwatch.StartNew();
            do
            {
                // Yes, not ideal - should be waiting on the file itself.
                try
                {
                    using (var f = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // OK, we have the file open.
                        using (var sr = new StreamReader(f))
                        {
                            ret = sr.ReadToEnd();
                            success = true;
                        }
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    SleepToRetry(); // too early!
                }
            } while (!success && sw.Elapsed < timeout);
            if (!success)
            {
                throw new TimeoutException(string.Format("Timeout waiting for {0} to be exclusively readable.", filename));
            }
            return ret;
        }

        private SystemUtilities.Shell32InstanceWrapper _sh;
        private SystemUtilities.DisposableTempPath _cab;

#region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cab.Dispose();
                }
                _cab = null;
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

#endregion IDisposable Support
    }

    public sealed class Shell32CabFileFactory : ICabFileFactory
    {
        public Shell32CabFileFactory()
        {
            _shell = new Sensics.SystemUtilities.Shell32InstanceWrapper();
        }

        public Shell32CabFileFactory(Shell32InstanceWrapper shell)
        {
            _shell = shell;
        }

        public ICabFile OpenCab(string filename)
        {
            return new Shell32CabFile(_shell, filename);
        }

        private Shell32InstanceWrapper _shell;
    }
}