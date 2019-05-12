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
    public Block BlockNow => GameManager.BlockNow;
    public Block BlockGhost => GameManager.BlockGhost;

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
      GameManager.TouchedDown += GameManager_Dropped;
      GameManager.Held += GameManager_Held;

      GameTimer.Elapsed += GameTimer_Elapsed;
      GameTimer.Start();
    }

    private bool IsFirstHeld = true;
    private bool IsHeld = false;
    private void GameManager_Held(object sender, EventArgs e)
    {
      IsHeld = true;
    }

    private bool IsDropped = false;
    private void GameManager_Dropped(object sender, EventArgs e)
    {
      IsDropped = true;
    }

    private void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      var gmOld = GameManager.Clone() as GameManager;
      lock (locker)
      {
        HandleInput();
        Update();
        Render();
      }

      void HandleInput()
      {
        var commands = InputManager.HandleInput();
        GameManager.HandleCommand(commands);
        StateManager.HandleCommand(commands, GameManager);
      }
      void Update()
      {
        GameManager.Update();
        StateManager.Update(GameManager);
      }
      void Render()
      {
        if (IsDropped || IsHeld && IsFirstHeld)
          OnPropertyChanged(nameof(BlockNexts));

        if (IsHeld)
          OnPropertyChanged(nameof(BlockHold));

        if (IsDropped)
          OnPropertyChanged(nameof(FieldCells));

        if (GameManager.BlockNow != gmOld.BlockNow)
        {
          OnPropertyChanged(nameof(BlockNow));
          OnPropertyChanged(nameof(BlockGhost));

        }

        if (IsDropped)
          IsDropped = false;
        if (IsHeld)
        {
          IsHeld = false;
          if (IsFirstHeld)
            IsFirstHeld = false;
        }
      }
    }

  }
}
