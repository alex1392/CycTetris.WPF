using System.Windows.Input;

namespace CycTetris.WPF
{
  public interface ITrackKeyState
  {
    Key PressedKey { get; }
  }
}