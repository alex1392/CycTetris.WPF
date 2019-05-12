using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class DownStates
  {
    public class NormalState : IHandleState
    {
      public IHandleState Handle(BlockCommand command, GameManager gm)
      {
        if (!command.IsPressed)
          return null;

        var (canExecute, blockExecuted) = gm.MoveCheck(command);
        if (!canExecute)
          return Constants.IsDLD ? new LockDelayState(command.Key) : null; 

        gm.BlockNow = blockExecuted;
        return new AutoShiftState(command.Key);
      }
    }

    public class LockDelayState : IHandleState, ITrackKeyState
    {
      public Key PressedKey { get; private set; }

      public LockDelayState(Key key)
      {
        PressedKey = key;
      }

      public int Delay { get; set; } = Constants.DLD;
      public static int DelayCount { get; set; } = 0;

      public IHandleState Handle(BlockCommand command, GameManager gm)
      {
        if (command.Key != PressedKey)
          return null;

        if (!command.IsPressed)
          return new NormalState();

        if (++DelayCount <= Delay)
          return null;

        if (!gm.IsDropped())
          return new NormalState();
        
        gm.Dropped();
        DelayCount = 0;
        return new NormalState();
      }
    }

    public class AutoShiftState : IHandleState, IDelayState, ITrackKeyState
    {
      public Key PressedKey { get; private set; }
      public int Delay { get; set; } = Constants.ASD;
      public AutoShiftState(Key key)
      {
        PressedKey = key;
      }

      public int DelayCount { get; set; } = 0;
      public IHandleState Handle(BlockCommand command, GameManager gm)
      {
        if (command.Key != PressedKey)
          return null;
        if (!command.IsPressed)
          return new NormalState();

        if (++DelayCount > Delay)
        {
          var (canExecute, blockExecuted) = gm.MoveCheck(command);
          if (!canExecute)
            return Constants.IsDLD ? new LockDelayState(command.Key) : null;
          gm.BlockNow = blockExecuted;
          DelayCount = 0;
        }
        return null;
      }
    }
  }
}
