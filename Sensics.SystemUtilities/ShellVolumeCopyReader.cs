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

        private sealed class CopyPerformer
        {
            public void Perform(Shell32InstanceWrapper shell, string destDir, Shell32.FolderItem src, TimeSpan timeout, CopyReaderDelegate reader)
            {
                var destFile = Path.GetFullPath(Path.Combine(destDir, Path.GetFileName(src.Path)));

                var destFolder = shell.NameSpace(destDir);
                StartCopy(src, destFolder);
                FinishCopy(destFile, timeout, reader);
            }
            public void StartCopy(Shell32.FolderItem src, Shell32.Folder destFolder)
            {
                destFolder.CopyHere(src, 0 /*4  do not show file copy dialog */);
            }

#if false
            public void StartCopy(Shell32.FolderItem src, Shell32.Folder destFolder)
            {
                using (GotStart = new ManualResetEventSlim())
                {
                    using (var fsw = new FileSystemWatcher(DestDir))
                    {
                        fsw.Created += HandleFSW;
                        fsw.Changed += HandleFSW;
                        destFolder.CopyHere(src, 0 /*4  do not show file copy dialog */);
                        // Do not proceed until file copy has at least begun.
                        GotStart.Wait();
                    }
                }
            }

            private void HandleFSW(object sender, FileSystemEventArgs e)
            {
                if (e.FullPath == DestFile)
                {
                    Debug.WriteLine("got changed event for the right file, setting reset event.");
                    GotStart.Set();
                }
                else
                {
                    Debug.WriteLine("Got changed event, wrong file {0}", e.FullPath);
                }
            }
            
#endif
            public void FinishCopy(string destFile, TimeSpan timeout, CopyReaderDelegate reader)
            {
                // OK, file copy has begun, now try loading.
                bool success = false;
                var sw = Stopwatch.StartNew();
                do
                {
                    // Yes, not ideal - should be waiting on the file itself.
                    try
                    {

                        Debug.WriteLine(string.Format("Trying to open {0}", destFile));
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

        public static void CopyItemHereAndRead(Shell32InstanceWrapper shell, string destDir, Shell32.FolderItem src, TimeSpan timeout, CopyReaderDelegate reader)
        {
            var performer = new CopyPerformer();
            performer.Perform(shell, destDir, src, timeout, reader);
#if false
            var destFile = Path.GetFullPath(Path.Combine(destDir, Path.GetFileName(src.Path)));
            var destFolder = shell.NameSpace(destDir);
            using (var gotChanged = new ManualResetEventSlim())
            {
                using (var fsw = new FileSystemWatcher(destDir))
                {
                    fsw.Changed += (sender, e) =>
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
#endif
        }

        private static void SleepToRetry()
        {
            // Yes, not ideal - should be waiting on the file itself.
            Thread.Sleep(20);
        }
    }
}