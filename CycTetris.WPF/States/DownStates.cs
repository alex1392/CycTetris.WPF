using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class DownStates
  {
    public class NormalState : IBlockState, IDropState
    {
      public bool IsDropped { get; set; } = false;

      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (!command.IsPressed)
          return null;

        var (canExecute, blockExecuted) = gm.MoveCheck(command);
        IsDropped = !canExecute;
        if (!canExecute)
          return new LockDelayState(command.Key);

        gm.BlockNow = blockExecuted;
        return new AutoShiftState(Constants.ASD, command.Key);
      }
    }

    public class LockDelayState : IBlockState, IDropState
    {
      public Key PressedKey { get; private set; }

      public LockDelayState(Key key)
      {
        PressedKey = key;
      }

      public int Delay { get; set; } = Constants.DLD;
      public static int DelayCount { get; set; } = 0;
      public bool IsDropped { get; set; } = false;

      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (command.Key != PressedKey)
          return null;

        if (!command.IsPressed)
          return new NormalState();

        if (++DelayCount <= Delay)
          return null;

        gm.Dropped();
        DelayCount = 0;
        return new NormalState();
      }
    }

    public class AutoShiftState : IBlockState, IDelayState, IDropState
    {
      public Key PressedKey { get; private set; }
      public int Delay { get; set; }
      public AutoShiftState(int delay, Key key)
      {
        Delay = delay;
        PressedKey = key;
      }

      public int DelayCount { get; set; } = 0;
      public bool IsDropped { get; set; } = false;
      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (command.Key != PressedKey)
          return null;
        if (!command.IsPressed)
          return new NormalState();

        if (++DelayCount > Delay)
        {
          var (canExecute, blockExecuted) = gm.MoveCheck(command);
          IsDropped = !canExecute;
          if (!canExecute)
            return new LockDelayState(command.Key);
          gm.BlockNow = blockExecuted;
          DelayCount = 0;
        }
        return null;
      }
    }
  }
}
