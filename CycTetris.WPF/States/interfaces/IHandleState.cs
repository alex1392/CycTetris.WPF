namespace CycTetris.WPF
{
  public interface IHandleState : IState
  {
    IState Handle(StateCommand command, GameManager gm);
  }
}
