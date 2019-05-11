using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public class EmptyState : IBlockState
  {
    public IBlockState HandleCommand(GameManager gm, BlockCommand command)
    {
      return null;
    }
  }
}
