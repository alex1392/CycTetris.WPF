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
    public BlockType[] CellList => GameManager.Field.Cells.Resize(colFirst: false);

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
        var blockOld = GameManager.BlockNow.Clone() as Block;
        InputManager.HandleInput();
        StateManager.HandleCommand(InputManager.BlockCommands, GameManager);
        return GameManager.Update(blockOld, 
          removeOld: (StateManager.DownState is IDropState dState) ? !(dState).IsDropped : true);

      }
      bool Update()
      {
        var blockOld = GameManager.BlockNow.Clone() as Block;
        StateManager.Update(GameManager);
        return GameManager.Update(blockOld, 
          removeOld: (StateManager.DropState is IDropState dState) ? !(dState).IsDropped : true);
      }
      void Render()
      {
        OnPropertyChanged(nameof(CellList));
      }
    }
  }
}
