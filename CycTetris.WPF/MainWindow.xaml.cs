using CycWpfLibrary;
using System.Collections.Generic;
using System.Linq;
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
