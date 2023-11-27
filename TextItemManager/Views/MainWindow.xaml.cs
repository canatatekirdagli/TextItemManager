using System.Windows;
using System.Windows.Controls;
using TextItemManager.ViewModels;

namespace TextItemManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.TextBoxTextChangedCommand.Execute(null);
            }
        }

        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}
