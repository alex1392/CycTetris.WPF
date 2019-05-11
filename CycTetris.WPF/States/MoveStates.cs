using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class MoveStates
  {
    public class NormalState : IBlockState
    {
      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (!command.IsPressed)
          return null;

        var (canExecute, blockExecuted) = gm.MoveCheck(command);
        if (!canExecute)
          return null;
        gm.BlockNow = blockExecuted;
        return new DasState(command.Key);
      }
    }

    public class DasState : IBlockState, IDelayState
    {
      public Key PressedKey { get; private set; }

      public DasState(Key key)
      {
        PressedKey = key;
      }

      public int Delay { get; set; } = Constants.DAS;
      public int DelayCount { get; set; } = 0;

      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (command.Key != PressedKey)
          return null;
        if (!command.IsPressed)
          return new NormalState();
        if (++DelayCount <= Delay)
          return null;
        return new AutoShiftState(Constants.ASD, command.Key);
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
          if (!canExecute)
          {
            return new NormalState();
          }
          gm.BlockNow = blockExecuted;
          DelayCount = 0;
        }
        return null;
      }
    }
  }
}
