using System;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class StateCommand : PlayerCommand
  {
    public StateCommandType Type { get; set; }

    public StateCommand(StateCommandType type, Key key, Action<GameManager> execute) 
      : base(execute)
    {
      Type = type;
      Key = key;
    }
  }
}
