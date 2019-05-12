using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class HardDropStates
  {
    public class NormalState : IHandleState
    {
      public static int count { get; set; } = 0;
      public IState Handle(BlockCommand command, GameManager gm)
      {
        if (!command.IsPressed)
          return null;

        gm.HardDrop();
        return new PressedState();
      }

    }

    public class PressedState : IHandleState
    {

      public IState Handle(BlockCommand command, GameManager gm)
      {
        if (command.IsPressed)
          return null;

        return new NormalState();
      }
    }
  }
}
