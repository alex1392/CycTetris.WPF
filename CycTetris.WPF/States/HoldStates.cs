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
      public IState Handle(StateCommand command, GameManager gm)
      {
        if (!command.IsPressed)
          return null;

        gm.Hold();
        return new HeldState();
      }
    }

    public class HeldState : IUpdateState, IEnterState, ILeaveState
    {
      public bool IsDropped { get; set; } = false;
      private void Gm_Dropped(object sender, EventArgs e)
      {
        IsDropped = true;
      }

      public IState Update(GameManager gm)
      {
        if (!IsDropped)
          return null;

        return new NormalState();
      }

      public void Enter(GameManager gm)
      {
        gm.TouchedDown += Gm_Dropped;
      }

      public void Leave(GameManager gm)
      {
        gm.TouchedDown -= Gm_Dropped;
      }
    }
  }
}
