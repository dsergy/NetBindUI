using NetBindUI.UI.ViewModels;
using System.Windows;

namespace NetBindUI.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}