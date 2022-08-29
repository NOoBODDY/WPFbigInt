using Microsoft.Win32;

namespace WPFbigInt.FileServices;

public class SaveFileDialogService: ISaveFileService
{
    public string OpenFileDialog(string defaultPath)
    {
        Microsoft.Win32.SaveFileDialog openFileWindow = new SaveFileDialog();
        openFileWindow.FileName = "document";
        openFileWindow.DefaultExt = ".txt";
        openFileWindow.Filter = "Text documents (.txt)|*.txt";
        if (openFileWindow.ShowDialog() == true)
            return openFileWindow.FileName;
        return defaultPath;
    }
}