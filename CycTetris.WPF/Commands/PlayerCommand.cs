using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public abstract class PlayerCommand : RelayCommand<GameManager>
  {
    protected PlayerCommand(Action<GameManager> execute) : base(execute)
    {
      
    }

    public bool IsPressed { get; set; }
    public Key Key { get; set; }

  }
}
