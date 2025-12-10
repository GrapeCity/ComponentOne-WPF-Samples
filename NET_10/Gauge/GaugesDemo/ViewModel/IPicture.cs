using System.Threading.Tasks;

namespace GaugesDemo
{
    public interface IPicture
    {
        void SavePictureToDisk(string filename, Task<byte[]> imageData);

    }
}
