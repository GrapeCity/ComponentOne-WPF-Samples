using System.Threading.Tasks;

namespace FlexGridExplorer
{
    public interface IFileSystem
    {
        Task SaveFileToDisk(string fileName, string data);
        Task<string> ReadFileFromDisk(string fileName);
    }
}
