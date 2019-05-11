using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public enum BlockType
  {
    // default
    None,
    // types
    Z, S, J, L, I, T, O,
    // should only be used for counting
    Count
  }
}
