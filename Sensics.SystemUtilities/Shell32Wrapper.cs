using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensics.SystemUtilities
{
    public class Shell32InstanceWrapper
    {
        /// <summary>
        /// The simple way of var sh = new Shell32.Shell(); is broken in win8.
        /// see http://stackoverflow.com/a/12077409/265522
        /// </summary>
        public Shell32InstanceWrapper()
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            _shell = Activator.CreateInstance(shellAppType);
        }

        public Shell32.Folder NameSpace(string folder)
        {
            return _shell.NameSpace(folder);
        }

        private dynamic _shell;
    }
}
