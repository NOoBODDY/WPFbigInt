using Microsoft.Win32;

namespace WPFbigInt.FileServices;

public class OpenFileDialogService: IOpenFileService
{
    public string OpenFileDialog(string defaultPath)
    {
        Microsoft.Win32.OpenFileDialog openFileWindow = new OpenFileDialog();
        openFileWindow.FileName = "document";
        openFileWindow.DefaultExt = ".txt";
        openFileWindow.Filter = "Text documents (.txt)|*.txt";
        if (openFileWindow.ShowDialog() == true)
            return openFileWindow.FileName;
        return defaultPath;
    }
    
}