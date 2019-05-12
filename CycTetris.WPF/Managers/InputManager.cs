using CycWpfLibrary.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CycTetris.WPF
{
  public class InputManager
  {
    public List<BlockCommand> BlockCommands { get; set; } = new List<BlockCommand>();

    public void Initialize()
    {
      BlockCommands.Add(new BlockCommand(BlockCommandType.Left, Key.Left, 
        gm => gm.BlockNow.Left()));
      BlockCommands.Add(new BlockCommand(BlockCommandType.Right, Key.Right, 
        gm => gm.BlockNow.Right()));
      BlockCommands.Add(new BlockCommand(BlockCommandType.Down, Key.Down, 
        gm => gm.BlockNow.Down()));
      BlockCommands.Add(new BlockCommand(BlockCommandType.RotateCW, Key.Up, 
        gm => gm.BlockNow.RotateCW()));
      BlockCommands.Add(new BlockCommand(BlockCommandType.RotateCCW, Key.X, 
        gm => gm.BlockNow.RotateCCW()));

      BlockCommands.Add(new BlockCommand(BlockCommandType.Hold, Key.Z,
        gm => gm.Hold()));
      BlockCommands.Add(new BlockCommand(BlockCommandType.HardDrop, Key.C,
        gm => gm.HardDrop()));
    }

    public List<BlockCommand> HandleInput()
    {
      foreach (var command in BlockCommands)
      {
        // Need to improve efficiency!!
        command.IsPressed = DispatchServices.Invoke(() => Keyboard.IsKeyDown(command.Key)); 
      }
      return BlockCommands;
    }
  }
}
