namespace Sensics.CabTools
{
    public sealed class DFTCabFileFactory : ICabFileFactory
    {
        public ICabFile OpenCab(string filename)
        {
            return new DFTCabFile(filename);
        }
    }
}