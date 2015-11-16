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
using System.IO;

namespace Sensics.SystemUtilities
{
    /// <summary>
    /// A uniform way to refer to directories and files, primarily for usage as temporary locations.
    /// </summary>
    interface IDeletableFileSystemObject
    {
        /// <summary>
        /// The (preferably full) path name
        /// </summary>
        string PathName { get; }
        /// <summary>
        /// The containing directory in the case of a file, otherwise the directory.
        /// </summary>
        string DirName { get; }

        /// <summary>
        /// Try to delete the file system object, catching harmless exceptions that don't interfere
        /// with cleaning up.
        /// </summary>
        void TryDelete();
    }

    public class DeletableFile : IDeletableFileSystemObject
    {
        public DeletableFile(string filename)
        {
            PathName = filename;
        }

        public string PathName { get; private set; }

        public string DirName
        {
            get
            {
                return Path.GetDirectoryName(PathName);
            }
        }

        public void TryDelete()
        {
            try
            {
                File.Delete(PathName);
            }
            catch (FileNotFoundException)
            {
                // that's OK
            }
            catch (IOException)
            {
                // also OK
            }
        }
    }

    public class DeletableDirectory : IDeletableFileSystemObject
    {
        public DeletableDirectory(string dirname)
        {
            DirName = dirname;
        }

        public string PathName
        {
            get
            {
                return DirName;
            }
        }

        public string DirName { get; private set; }

        public void TryDelete()
        {
            try
            {
                Directory.Delete(PathName, true);
            }
            catch (DirectoryNotFoundException)
            {
                // that's OK
            }
            catch (IOException)
            {
                // also OK
            }
        }
    }
}
