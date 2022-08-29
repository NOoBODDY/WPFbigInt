using System.Threading.Tasks;

namespace WPFbigInt.FileServices;

public interface IFileService
{
    void ChooseOpenFilePath();

    bool IsReadingFileChosen();
    void ChooseSaveFilePath();
    bool IsWritingFileChosen();

    string ReadFileLine();

    void CloseReadingFile();
    void WriteFileLine(string line);
    
    Task<string> ReadFileLineAsync();
    Task WriteFileLineAsync(string line);
    
    
}