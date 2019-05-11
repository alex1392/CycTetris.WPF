using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycTetris.WPF
{
  public class StateManager
  {
    public List<IState> States = new List<IState>
    {
      new MoveStates.NormalState(),
      new RotateStates.NormalState(),
      new DownStates.NormalState(),
      new DropStates.NormalState(),
      new EmptyState(),
    };
    public static Dictionary<BlockCommandType, int> IndexDict = new Dictionary<BlockCommandType, int>
    {
      { BlockCommandType.Left, 0 },
      { BlockCommandType.Right, 0 },
      { BlockCommandType.RotateCW, 1 },
      { BlockCommandType.RotateCCW, 1 },
      { BlockCommandType.Down, 2 },

      { BlockCommandType.HardDrop, 4 },
      { BlockCommandType.Hold, 4 },
    };

    public IHandleState MoveState
    {
      get => States[IndexDict[BlockCommandType.Left]] as IHandleState;
      set => States[IndexDict[BlockCommandType.Left]] = value;
    }
    public IHandleState RotateState
    {
      get => States[IndexDict[BlockCommandType.RotateCW]] as IHandleState;
      set => States[IndexDict[BlockCommandType.RotateCW]] = value;
    }
    public IHandleState DownState
    {
      get => States[IndexDict[BlockCommandType.Down]] as IHandleState;
      set => States[IndexDict[BlockCommandType.Down]] = value;
    }
    public IState DropState
    {
      get => States[3] as IState;
      set => States[3] = value;
    }

    public void Initialize()
    {

    }

    public void Handle(List<BlockCommand> commands, GameManager gm)
    {
      foreach (var command in commands)
      {
        var newState = (States[IndexDict[command.Type]] as IHandleState).Handle(command, gm);
        if (newState is null)
          continue;
        States[IndexDict[command.Type]] = newState;
      }
    }

    public void Update(GameManager gm)
    {
      for (int i = 0; i < States.Count; i++)
      {
        var state = States[i];
        if (!(state is IUpdateState uState))
          continue;

        var newState = uState.Update(gm);
        if (newState is null)
          continue;
        States[i] = newState;
      }
    }
  }

  
}
