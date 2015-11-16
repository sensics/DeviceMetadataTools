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
using Sensics.SystemUtilities;
using System;
using System.IO;

namespace Sensics.CabTools
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
            var searchingFor = Path.Combine(_cab.PathName, contained);
            Shell32.FolderItem srcItem = null;

            // parseName didn't seem to work properly with subdirectories.
            foreach (Shell32.FolderItem item in cabFolder.Items())
            {
                //Debug.WriteLine(string.Format("Got item {0}", item.Path));
                if (item.Path == searchingFor)
                {
                    srcItem = item;
                }
            }
            if (srcItem == null)
            {
                throw new FileNotFoundException(string.Format("Could not find file '{0}' inside cab file '{1}'", contained, _cab.PathName));
            }
            string text = "";
            using (var destDir = DisposableTempPath.CreateUniqueDirectory())
            {
                var destFolder = _sh.NameSpace(destDir.PathName);
                var tempFile = Path.Combine(destDir.PathName, Path.GetFileName(contained));

                ShellVolumeCopyReader.CopyItemHereAndRead(_sh, destDir.PathName, srcItem, TimeSpan.FromSeconds(.5),
                    (stream) =>
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            text = sr.ReadToEnd();
                        }
                    });
            }

            return new StringReader(text);
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
}