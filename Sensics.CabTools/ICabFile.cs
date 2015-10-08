using System.IO;

namespace Sensics.CabTools
{
    /// <summary>
    /// Interface allowing you to read the context of a text file contained in a cab.
    /// Implementations may open the cab but may not hold it open or hold it exclusively.
    /// </summary>
    public interface ICabFile
    {
        TextReader OpenTextFile(string contained);
    }
}
