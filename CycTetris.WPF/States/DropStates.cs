using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public class DropStates
  {
    public class NormalState : IUpdateState, IDropState
    {
      public NormalState()
      {

      }
      public NormalState(int delayCount)
      {
        DelayCount = delayCount;
      }

      public int Delay { get; set; } = Constants.DT;
      public int DelayCount { get; set; } = 0;
      public bool IsDropped { get; set; } = false;

      public IState Update(GameManager gm)
      {
        IsDropped = gm.IsDropped();
        if (IsDropped)
          return new LockDelayState();

        if (++DelayCount <= Delay)
          return null;

        DelayCount = 0;
        gm.BlockNow.Down();
        return null;
      }
    }

    public class LockDelayState : IUpdateState, IDropState
    {
      public int Delay { get; set; } = Constants.LD;
      public static int DelayCount { get; set; } = 0;
      public bool IsDropped { get; set; } = false;

      public IState Update(GameManager gm)
      {
        IsDropped = gm.IsDropped();
        if (!IsDropped)
          return new NormalState(DelayCount);

        if (++DelayCount <= Delay)
          return null;

        gm.Dropped();
        DelayCount = 0;
        return new NormalState();
      }
    }
  }
}