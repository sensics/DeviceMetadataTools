using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sensics.SystemUtilities
{
    static public class ShellVolumeCopyReader
    {
        /// <summary>
        /// The type of delegate passed in to be called once a Shell32 CopyHere completes.
        /// When this delegate returns, the supplied FileStream is disposed of.
        /// </summary>
        /// <param name="stream">The file stream to use - do not copy outside of delegate.</param>
        public delegate void CopyReaderDelegate(FileStream stream);

        private static void CopyItemHereAndRead(Shell32InstanceWrapper shell, string destDir, Shell32.FolderItem src, CopyReaderDelegate reader, TimeSpan timeout)
        {
            var destFile = Path.GetFullPath(Path.Combine(destDir, Path.GetFileName(src.Path)));
            var destFolder = shell.NameSpace(destDir);
            using (var gotChanged = new ManualResetEventSlim())
            {
                using (var fsw = new FileSystemWatcher(destDir))
                {
                    fsw.Changed += delegate (object sender, FileSystemEventArgs e)

                   {
                       if (e.FullPath == destFile)
                       {
                           Debug.WriteLine("got changed event for the right file, setting reset event.");
                           gotChanged.Set();
                       }
                       else
                       {
                           Debug.WriteLine("Got changed event, wrong file {0}", e.FullPath);
                       }
                   };

                    destFolder.CopyHere(src, 0 /*4  do not show file copy dialog */);
                    // Do not proceed until file copy has at least begun.
                    gotChanged.Wait();
                }
            }

            // OK, file copy has begun, now try loading.
            bool success = false;
            var sw = Stopwatch.StartNew();
            do
            {
                // Yes, not ideal - should be waiting on the file itself.
                try
                {
                    using (var f = File.Open(destFile, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        // OK, we have the file open. Call the delegate!
                        success = true;
                        reader(f);
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    SleepToRetry(); // too early!
                }
            } while (!success && sw.Elapsed < timeout);

            if (!success)
            {
                throw new TimeoutException(string.Format("Timeout waiting for {0} to be exclusively readable.", destFile));
            }
        }
        private static void SleepToRetry()
        {
            // Yes, not ideal - should be waiting on the file itself.
            Thread.Sleep(20);
        }

    }
}