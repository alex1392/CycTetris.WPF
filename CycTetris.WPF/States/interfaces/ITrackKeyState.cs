using System.Windows.Input;

namespace CycTetris.WPF
{
  public interface ITrackKeyState : IState
  {
    Key PressedKey { get; }
  }
}