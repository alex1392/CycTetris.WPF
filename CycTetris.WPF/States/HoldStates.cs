using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public class HoldStates
  {
    public class NormalState : IHandleState
    {
      public IState Handle(BlockCommand command, GameManager gm)
      {
        if (!command.IsPressed)
          return null;

        gm.Hold();
        return new HeldState();
      }
    }

    public class HeldState : IUpdateState
    {
      public IState Update(GameManager gm)
      {
        if (!gm.IsDropped)
          return null;

        return new NormalState();
      }
    }
  }
}
