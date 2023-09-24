using KpblcCadCore.ViewModels;
using System.Windows;

namespace KpblcCadCore.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для PaintAttrAndBlockWindow.xaml
    /// </summary>
    public partial class PaintAttrAndBlockWindow : Window
    {
        public PaintAttrAndBlockWindow()
        {
            InitializeComponent();
            IniDataContext();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            IniDataContext();
            _viewModel.OnOkButtonClick();
            Close();
        }

        BlockAttributeColoringViewModel _viewModel;

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            IniDataContext();
            _viewModel.OnCancelButtonClick();
            Close();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            IniDataContext();
        }

        private void IniDataContext()
        {
            _viewModel = this.DataContext as BlockAttributeColoringViewModel;
        }
    }
}
