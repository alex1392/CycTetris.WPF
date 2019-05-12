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
      new HoldStates.NormalState(),
    };
    public static Dictionary<BlockCommandType, int> IndexDict = new Dictionary<BlockCommandType, int>
    {
      { BlockCommandType.Left, 0 },
      { BlockCommandType.Right, 0 },
      { BlockCommandType.RotateCW, 1 },
      { BlockCommandType.RotateCCW, 1 },
      { BlockCommandType.Down, 2 },
      { BlockCommandType.Hold, 4 },
    };

    public void Initialize()
    {

    }

    public void HandleCommand(List<BlockCommand> commands, GameManager gm)
    {
      foreach (var command in commands)
      {
        if (!IndexDict.ContainsKey(command.Type))
          continue;
        if (!(States[IndexDict[command.Type]] is IHandleState hState))
          continue;

        var newState = hState.Handle(command, gm);
        if (newState is null)
          continue;

        if (newState is IEnterState eState)
          eState.Enter(gm);
        if (States[IndexDict[command.Type]] is ILeaveState lState)
          lState.Leave(gm);

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

        if (newState is IEnterState eState)
          eState.Enter(gm);
        if (States[i] is ILeaveState lState)
          lState.Leave(gm);

        States[i] = newState;
      }
    }
  }
}
