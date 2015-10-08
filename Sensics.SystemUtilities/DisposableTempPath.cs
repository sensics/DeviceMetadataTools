using System;
using System.IO;

namespace Sensics.SystemUtilities
{
    /// <summary>
    /// Takes care of deleting a temp file/directory when you're done with it.
    /// Be sure to use "using" or other IDisposable-approved methods or you lose the deterministic destruction/cleanup properties.
    /// </summary>
    public sealed partial class DisposableTempPath : IDisposable
    {
        #region Factory functions

        /// <summary>
        /// Factory function: Creates a directory with a statistically-unique name, and takes responsibility for deleting it.
        /// </summary>
        /// <returns>Instance that will delete the temporary directory upon disposal</returns>
        public static DisposableTempPath CreateUniqueDirectory()
        {
            var dirname = TempUtils.GetFullUniquePath();
            Directory.CreateDirectory(dirname);
            return new DisposableTempPath(new DeletableDirectory(dirname));
        }

        /// <summary>
        /// Factory function: Generates a statistically-unique filename and takes responsibility for deleting it. Creating it is your responsibility.
        /// </summary>
        /// <returns>Instance responsible for deleting the temporary file if you create it there</returns>
        public static DisposableTempPath GenerateUniqueFilename(string extension = ".tmp")
        {
            return new DisposableTempPath(new DeletableFile(TempUtils.GetFullUniquePath(extension)));
        }

        /// <summary>
        /// Factory function: copies the given file to a statistically-unique temporary filename. Will not overwrite in the negligible chance of a collision.
        /// </summary>
        /// <param name="src">Source file path</param>
        /// <param name="extension">Optional, lets you specify the extension of your temporary file (leading . required)</param>
        /// <returns>Instance that will delete the temporary copy upon disposal</returns>
        public static DisposableTempPath CopyToUniqueFilename(string src, string extension = ".tmp")
        {
            return CopyToUniqueFilename(src, extension, false);
        }

        /// <summary>
        /// Factory function: copies the given file to a statistically-unique temporary filename. Can optionally overwrite in the negligible chance of a collision.
        /// </summary>
        /// <param name="src">Source file path</param>
        /// <param name="extension">Optional, lets you specify the extension of your temporary file (leading . required)</param>
        /// <param name="overwrite">Whether the copy process should overwrite an existing file in the negligible chance of a filename collision</param>
        /// <returns>DisposableTempFile instance that will delete the temporary copy upon disposal</returns>
        public static DisposableTempPath CopyToUniqueFilename(string src, string extension = ".tmp", bool overwrite = false)
        {
            var newFile = TempUtils.GetFullUniquePath(extension);
            File.Copy(src, newFile, overwrite);
            return new DisposableTempPath(new DeletableFile(newFile));
        }

        #endregion Factory functions

        #region Public properties and methods

        /// <summary>
        /// The full path of the temporary file/directory. Throws if we've already disposed of (or
        /// released control of) this object.
        /// </summary>
        public string PathName
        {
            get
            {
                if (disposedValue)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                return _tempObj.PathName;
            }
        }

        /// <summary>
        /// The directory of the temporary object: the parent of the temp file, or the directory
        /// itself. Throws if we've already disposed of (or released control of) this object.
        /// </summary>
        public string DirName
        {
            get
            {
                if (disposedValue)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }
                return _tempObj.DirName;
            }
        }

        /// <summary>
        /// Stop managing the object - that is, don't delete it when we are disposed of.
        /// </summary>
        public void ReleaseControl()
        {
            // actually just pretend we already disposed of it.
            disposedValue = true;
        }

        #endregion Public properties and methods

        #region Private instance methods

        private DisposableTempPath(IDeletableFileSystemObject temp)
        {
            _tempObj = temp;
        }

        #endregion Private instance methods

        #region Private members
        
        private IDeletableFileSystemObject _tempObj;

        #endregion Private members

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls and post-disposal use

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                if (_tempObj != null) { 
                    // A file is effectively an "unmanaged resource"
                    _tempObj.TryDelete();
                }
                disposedValue = true;
            }
            _tempObj = null;
        }

        ~DisposableTempPath()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}