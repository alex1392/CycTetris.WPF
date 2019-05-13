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
    public List<StateCommand> StateCommands { get; set; } = new List<StateCommand>();
    public List<PressCommand> PressCommands { get; set; } = new List<PressCommand>();

    public void Initialize()
    {
      StateCommands.Add(new StateCommand(StateCommandType.Left, Key.Left, 
        gm => gm.BlockNow.Left()));
      StateCommands.Add(new StateCommand(StateCommandType.Right, Key.Right, 
        gm => gm.BlockNow.Right()));
      StateCommands.Add(new StateCommand(StateCommandType.Down, Key.Down, 
        gm => gm.BlockNow.Down()));

      PressCommands.Add(new PressCommand(PressCommandType.Hold, Key.Z, gm => gm.Hold()));

      PressCommands.Add(new PressCommand(PressCommandType.HardDrop, Key.C, gm => gm.HardDrop()));
      PressCommands.Add(new PressCommand(PressCommandType.RotateCW, Key.Up, gm => gm.BlockNow.RotateCW()));
      PressCommands.Add(new PressCommand(PressCommandType.RotateCCW, Key.X, gm => gm.BlockNow.RotateCCW()));
      PressCommands.Add(new PressCommand(PressCommandType.Reset, Key.Escape, gm => gm.Reset()));
    }

    public List<StateCommand> HandleStateCommand()
    {
      foreach (var command in StateCommands)
      {
        command.IsPressed = DispatchServices.Invoke(() => Keyboard.IsKeyDown(command.Key)); 
      }
      return StateCommands;
    }

    public PressCommand HandleKeyDown(KeyEventArgs e)
    {
      var command = PressCommands.Find(c => e.Key == c.Key);
      if (command != null && !command.IsPressed)
        command.IsPressed = true;
      return command;
    }

    public PressCommand HandleKeyUp(KeyEventArgs e)
    {
      var command = PressCommands.Find(c => e.Key == c.Key);
      if (command != null && command.IsPressed)
      {
        command.IsPressed = false;
        command.IsHandled = false;
      }
      return command;
    }
  }
}
