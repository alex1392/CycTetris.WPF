using CycWpfLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class BlockCommand : RelayCommand<GameManager>, IPlayerCommand
  {
    public BlockCommandType Type { get; set; }
    public Key Key { get; set; }
    public bool IsPressed { get; set; }

    public BlockCommand(BlockCommandType type, Key key, Action<GameManager> execute) 
      : base(execute)
    {
      Type = type;
      Key = key;
    }
  }
}
