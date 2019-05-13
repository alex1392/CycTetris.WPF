namespace CycTetris.WPF
{
  public interface IDelayState : IState
  {
    int Delay { get; set; }
    int DelayCount { get; set; }
  }
}
