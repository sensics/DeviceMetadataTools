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
