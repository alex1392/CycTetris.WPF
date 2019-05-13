using System.Collections.Generic;

namespace CycTetris.WPF
{
  public class StateManager
  {
    public List<IState> States = new List<IState>
    {
      new MoveStates.NormalState(),
      new DownStates.NormalState(),
      new FallStates.NormalState(),
    };
    public static Dictionary<StateCommandType, int> IndexDict = new Dictionary<StateCommandType, int>
    {
      { StateCommandType.Left, 0 },
      { StateCommandType.Right, 0 },
      { StateCommandType.Down, 1 },
    };

    public void Initialize()
    {

    }

    public void HandleStateCommand(List<StateCommand> commands, GameManager gm)
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
      for (var i = 0; i < States.Count; i++)
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
