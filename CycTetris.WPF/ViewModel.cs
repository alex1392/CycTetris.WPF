using CycWpfLibrary;
using CycWpfLibrary.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static CycTetris.WPF.Constants;

namespace CycTetris.WPF
{
  public class ViewModel : ViewModelBase
  {
    public BlockType[] FieldCells => GameManager.Field.Cells.Resize(colFirst: false);
    public Block[] BlockNexts => GameManager.BlockNexts.ToArray();
    public Block BlockHold => GameManager.BlockHold;

    private readonly InputManager InputManager = new InputManager();
    private readonly StateManager StateManager = new StateManager();
    private readonly GameManager GameManager = new GameManager();
    private readonly Timer GameTimer = new Timer(1000d / FPS);
    private static readonly object locker = new object();

    public ViewModel()
    {
      InputManager.Initialize();
      StateManager.Initialize();
      GameManager.Initialize();

      GameTimer.Elapsed += GameTimer_Elapsed;
      GameTimer.Start();
    }

    private void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      lock (locker)
      {
        var hasInput = HandleInput();
        var isUpdated = Update();
        if (hasInput || isUpdated)
          Render();
      }

      bool HandleInput()
      {
        GameManager.RecordState();
        var commands = InputManager.HandleInput();
        GameManager.HandleCommand(commands);
        StateManager.HandleCommand(commands, GameManager);
        return GameManager.Update();
      }
      bool Update()
      {
        GameManager.RecordState();
        StateManager.Update(GameManager);
        return GameManager.Update();
      }
      void Render()
      {
        OnPropertyChanged(nameof(FieldCells));
        OnPropertyChanged(nameof(BlockNexts));
        OnPropertyChanged(nameof(BlockHold));
      }
    }
  }
}
