using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class RotateStates
  {
    public class NormalState : IBlockState
    {
      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (!command.IsPressed)
          return null;

        var funcs = new Collection<Func<BlockCommand, (bool, Block)>>
        {
          gm.MoveCheck, gm.KickCheck
        };
        foreach (var func in funcs)
        {
          var (canExecute, blockExecuted) = func(command);
          if (canExecute)
          {
            gm.BlockNow = blockExecuted;
            return new RotatedState(command.Key);
          }
        }
        return null;
      }
    }

    public class RotatedState : IBlockState
    {
      public Key PressedKey { get; private set; }

      public RotatedState(Key key)
      {
        this.PressedKey = key;
      }

      public IBlockState HandleCommand(GameManager gm, BlockCommand command)
      {
        if (command.Key != PressedKey)
          return null;
        if (!command.IsPressed)
          return new NormalState();
        return null;
      }
    }
  }
}
