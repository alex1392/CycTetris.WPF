using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public interface IDelayState : IState
  {
    int Delay { get; set; }
    int DelayCount { get; set; }
  }
}
