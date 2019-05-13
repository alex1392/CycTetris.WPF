using System;
using System.Timers;
using System.Windows.Input;
using CycWpfLibrary;
using static CycTetris.WPF.Constants;

namespace CycTetris.WPF
{
  public class ViewModel : ViewModelBase
  {
    public BlockType[] FieldCells => _gameManager.Field.Cells.Resize(colFirst: false);
    // ReSharper disable once IdentifierTypo
    public Block[] BlockNexts => _gameManager.BlockNexts.ToArray();
    public Block BlockHold => _gameManager.BlockHold;
    public Block BlockNow => _gameManager.BlockNow;
    public Block BlockGhost => _gameManager.BlockGhost;
    public void RenderAll()
    {
      OnPropertyChanged(nameof(BlockNexts));
      OnPropertyChanged(nameof(BlockHold));
      OnPropertyChanged(nameof(FieldCells));
      OnPropertyChanged(nameof(BlockNow));
      OnPropertyChanged(nameof(BlockGhost));
    }

    public ICommand KeyDownCommand { get; }
    public ICommand KeyUpCommand { get; }

    private readonly InputManager _inputManager = new InputManager();
    private readonly StateManager _stateManager = new StateManager();
    private readonly GameManager _gameManager = new GameManager();
    private readonly Timer _gameTimer = new Timer(1000d / FPS);
    private static readonly object locker = new object();

    public ViewModel()
    {
      _inputManager.Initialize();
      _stateManager.Initialize();
      _gameManager.Initialize();
      _gameManager.TouchedDown += GameManager_TouchedDown;

      KeyUpCommand = new RelayCommand<KeyEventArgs>(KeyUp);
      KeyDownCommand = new RelayCommand<KeyEventArgs>(KeyDown);

      _gameTimer.Elapsed += GameTimer_Elapsed;
      _gameTimer.Start();
    }

    private bool _isTouchedDown;
    private void GameManager_TouchedDown(object sender, EventArgs e)
    {
      _isTouchedDown = true;
    }

    private bool _isFirstHeld = true;
    /// <summary>
    /// Handle single-press commands
    /// </summary>
    private void KeyDown(KeyEventArgs e)
    {
      var command = _inputManager.HandleKeyDown(e);
      var isRender = _gameManager.HandlePressCommand(command);
      if (isRender)
        PressRender(command);
    }
    private void PressRender(PressCommand command)
    {
      switch (command.Type)
      {
        case PressCommandType.RotateCw:
        case PressCommandType.RotateCcw:
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
          if (_isFirstHeld)
          {
            OnPropertyChanged(nameof(BlockNexts));
            _isFirstHeld = false;
          }
          OnPropertyChanged(nameof(BlockHold));
          OnPropertyChanged(nameof(BlockNow));
          OnPropertyChanged(nameof(BlockGhost));
          break;
      }
    }
    private void KeyUp(KeyEventArgs e)
    {
      _inputManager.HandleKeyUp(e);
    }

    private void GameTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
      var gmOld = _gameManager.Clone() as GameManager;
      lock (locker)
      {
        HandleStateCommand();
        StateUpdate();
        StateRender(gmOld);
      }  
    }
    private void HandleStateCommand()
    {
      var commands = _inputManager.HandleStateCommand();
      _gameManager.HandleStateCommand(commands);
      _stateManager.HandleStateCommand(commands, _gameManager);
    }
    private void StateUpdate()
    {
      _gameManager.Update();
      _stateManager.Update(_gameManager);
    }
    private void StateRender(GameManager gmOld)
    {
      if (_isTouchedDown)
      {
        OnPropertyChanged(nameof(BlockNexts));
        OnPropertyChanged(nameof(FieldCells));
        _isTouchedDown = false;
      }

      if (_gameManager.BlockNow != gmOld.BlockNow)
      {
        OnPropertyChanged(nameof(BlockNow));
        OnPropertyChanged(nameof(BlockGhost));
      }
    }
  }
}
