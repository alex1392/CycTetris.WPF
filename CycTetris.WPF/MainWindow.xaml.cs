using System.Windows;

namespace CycTetris.WPF
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new ViewModel();
    }
  }
}
