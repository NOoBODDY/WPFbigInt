using System;

namespace WPFbigInt.FileServices;

public interface IOpenFileService
{
    string OpenFileDialog(string defaultPath);

}