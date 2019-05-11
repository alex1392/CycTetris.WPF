using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public class EmptyState : IHandleState
  {
    public IHandleState Handle(BlockCommand command, GameManager gm)
    {
      return null;
    }
  }
}
