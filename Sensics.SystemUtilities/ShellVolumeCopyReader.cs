#region copyright
// Copyright 2015 Sensics, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
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
        }
    }
}