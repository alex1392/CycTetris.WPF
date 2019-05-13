using System.Windows.Input;

namespace CycTetris.WPF
{
  public class MoveStates
  {
    public class NormalState : IHandleState
    {
      public IState Handle(StateCommand command, GameManager gm)
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

    public class DasState : IHandleState, IDelayState, ITrackKeyState
    {
      public Key PressedKey { get; }

      public DasState(Key key)
      {
        PressedKey = key;
      }

      public int Delay { get; set; } = Constants.DAS;
      public int DelayCount { get; set; }

      public IState Handle(StateCommand command, GameManager gm)
      {
        if (command.Key != PressedKey)
          return null;
        if (!command.IsPressed)
          return new NormalState();
        if (++DelayCount <= Delay)
          return null;
        return new AutoShiftState(command.Key);
      }
    }

    public class AutoShiftState : IHandleState, IDelayState, ITrackKeyState
    {
      public Key PressedKey { get; }
      public int Delay { get; set; } = Constants.ASD;
      public AutoShiftState(Key key)
      {
        PressedKey = key;
      }

      public int DelayCount { get; set; }
      public IState Handle(StateCommand command, GameManager gm)
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
