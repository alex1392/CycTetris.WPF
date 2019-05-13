using System;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class PressCommand : PlayerCommand
  {
    public PressCommand(PressCommandType type, Key c, Action<GameManager> execute) : base(execute)
    {
      Type= type;
      Key = c;
    }
    
    public PressCommandType Type { get; set; }
    public bool IsHandled { get; set; } = false;
  }
}