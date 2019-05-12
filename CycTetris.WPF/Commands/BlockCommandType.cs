using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public enum BlockCommandType
  {
    Left,
    Right,
    Down,
    RotateCW,
    RotateCCW,
    HardDrop,
    Hold,
    Reset,
  }
}
