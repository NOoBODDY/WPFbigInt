using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFbigInt.FileServices;
using WPFbigInt.Services;
using WPFbigInt.ViewModels;
using WPFbigInt.Views;

namespace WPFbigInt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }
        
        [STAThread]
        static void Main()
        {
            App app = new App();
            MainWindow window = new MainWindow();
            
            WindowsFileService fileService = new WindowsFileService( new OpenFileDialogService(),new SaveFileDialogService());
            MessageBoxUserNotyfication notyficationService = new MessageBoxUserNotyfication();
            window.DataContext = new MainViewModel(fileService, notyficationService);
            app.Run(window);
        }
        
    }
}