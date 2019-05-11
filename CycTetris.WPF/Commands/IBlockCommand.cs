using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public interface IPlayerCommand
  {
    bool IsPressed { get; set; }
    Key Key { get; set; }
    BlockCommandType Type { get; set; }
  }
}
