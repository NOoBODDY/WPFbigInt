using System;
using System.IO;
using System.Threading.Tasks;

namespace WPFbigInt.FileServices;

public class WindowsFileService:IFileService, IDisposable
{
    private readonly IOpenFileService _openFileService;
    private readonly ISaveFileService _saveFileService;

    private string? _readPath;
    private string? _writePath;
    private StreamReader? reader;
    public WindowsFileService(IOpenFileService openFileService, ISaveFileService saveFileService)
    {
        _openFileService = openFileService;
        _saveFileService = saveFileService;
    }

    public void ChooseOpenFilePath()
    {
        _readPath = _openFileService.OpenFileDialog("c://document.txt");
        FileInfo file = new FileInfo(_readPath);
        if (!file.Exists)
            throw new Exception($@"file {_readPath} does not exist");
    }

    public bool IsReadingFileChosen()
    {
        return _readPath != null;
    }

    public void ChooseSaveFilePath()
    {
        _writePath = _saveFileService.OpenFileDialog("c://document.txt");
        FileInfo file = new FileInfo(_writePath);
        if (!file.Exists)
            file.Create();
    }

    public bool IsWritingFileChosen()
    {
        return _writePath != null;
    }

    public string ReadFileLine()
    {
        if (reader == null)
            reader = new StreamReader(_readPath);
        return reader.ReadLine();
    }

    public void CloseReadingFile()
    {
        reader.Dispose();
        reader = null;
        _readPath = null;
    }

    public void WriteFileLine(string line)
    {
        using (StreamWriter writer = new StreamWriter(_writePath, true))
        {
            writer.WriteLine(line);
        }
    }

    public async Task<string> ReadFileLineAsync()
    {
        using (StreamReader reader = new StreamReader(_readPath))
        {
            return await reader.ReadLineAsync();
        }
    }

    public async Task WriteFileLineAsync(string line)
    {
        using (StreamWriter writer = new StreamWriter(_writePath, true))
        {
            await writer.WriteLineAsync(line);
        }
    }

    public void Dispose()
    {
        reader?.Dispose();
    }
}