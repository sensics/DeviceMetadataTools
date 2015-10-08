namespace Sensics.CabTools
{
    /// <summary>
    /// Provides a way to open a CAB file with a given backend, which might require additional
    /// arguments, while maintaining a constant interface to the consumer.
    /// </summary>
    public interface ICabFileFactory
    {
        ICabFile OpenCab(string filename);
    }
}
