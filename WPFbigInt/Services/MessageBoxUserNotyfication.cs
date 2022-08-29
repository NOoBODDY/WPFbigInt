using System.Windows;

namespace WPFbigInt.Services;

public class MessageBoxUserNotyfication: IUserNotyfication
{
    public void SendMessage(string message)
    {
        MessageBox.Show(message);
    }
}