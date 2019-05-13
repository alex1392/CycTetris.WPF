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
    public void RenderAll()
    {
      OnPropertyChanged(nameof(BlockNexts));
      OnPropertyChanged(nameof(BlockHold));
      OnPropertyChanged(nameof(FieldCells));
      OnPropertyChanged(nameof(BlockNow));
      OnPropertyChanged(nameof(BlockGhost));
    }

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
      GameManager.TouchedDown += GameManager_TouchedDown;

      KeyUpCommand = new RelayCommand<KeyEventArgs>(KeyUp);
      KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);

      GameTimer.Elapsed += GameTimer_Elapsed;
      GameTimer.Start();
    }

    private bool IsFirstHeld = true;

    private bool IsTouchedDown = false;
    private void GameManager_TouchedDown(object sender, EventArgs e)
    {
      IsTouchedDown = true;
    }

    public ICommand KeyDownCommand { get; private set; }
    /// <summary>
    /// Handle single-press commands
    /// </summary>
    private void KeyDown(KeyEventArgs e)
    {
      var command = InputManager.HandleKeyDown(e);
      var isRender = GameManager.HandlePressCommand(command);
      if (isRender)
      {
        switch (command.Type)
        {
          case PressCommandType.RotateCW:
          case PressCommandType.RotateCCW:
            OnPropertyChanged(nameof(BlockNow));
            OnPropertyChanged(nameof(BlockGhost));
            break;
          case PressCommandType.HardDrop:
            RenderAll();
            break;
          case PressCommandType.Reset:
            RenderAll();
            break;
          case PressCommandType.Hold:
            if (IsFirstHeld)
            {
              OnPropertyChanged(nameof(BlockNexts));
              IsFirstHeld = false;  
            }
            OnPropertyChanged(nameof(BlockHold));
            OnPropertyChanged(nameof(BlockNow));
            OnPropertyChanged(nameof(BlockGhost));
            break;
        }
      }
    }
    public ICommand KeyUpCommand { get; private set; }
    private void KeyUp(KeyEventArgs e)
    {
      InputManager.HandleKeyUp(e);
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
        var commands = InputManager.HandleStateCommand();
        GameManager.HandleStateCommand(commands);
        StateManager.HandleCommand(commands, GameManager);
      }
      void Update()
      {
        GameManager.Update();
        StateManager.Update(GameManager);
      }
      void Render()
      {
        if (IsTouchedDown)
        {
          OnPropertyChanged(nameof(BlockNexts));
          OnPropertyChanged(nameof(FieldCells));
          IsTouchedDown = false;
        }

        if (GameManager.BlockNow != gmOld.BlockNow)
        {
          OnPropertyChanged(nameof(BlockNow));
          OnPropertyChanged(nameof(BlockGhost));
        }
      }
    }
  }
}
