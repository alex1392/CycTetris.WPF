namespace CycTetris.WPF
{
  public interface IDropState : IState
  {
    bool IsDropped { get; set; }
  }
}