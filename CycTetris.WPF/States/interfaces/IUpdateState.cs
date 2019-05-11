namespace CycTetris.WPF
{
  public interface IUpdateState : IState
  {
    IState Update(GameManager gm);
  }
}