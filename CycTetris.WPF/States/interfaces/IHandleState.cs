﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public interface IHandleState : IState
  {
    IState Handle(BlockCommand command, GameManager gm);
  }
}
