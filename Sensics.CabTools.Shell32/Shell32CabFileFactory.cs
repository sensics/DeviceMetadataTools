using Sensics.SystemUtilities;

namespace Sensics.CabTools
{

    public sealed class Shell32CabFileFactory : ICabFileFactory
    {
        public Shell32CabFileFactory()
        {
            _shell = new Shell32InstanceWrapper();
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
