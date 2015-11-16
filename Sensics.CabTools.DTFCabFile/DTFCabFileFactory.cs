namespace Sensics.CabTools
{
    public sealed class DTFCabFileFactory : ICabFileFactory
    {
        public ICabFile OpenCab(string filename)
        {
            return new DTFCabFile(filename);
        }
    }
}