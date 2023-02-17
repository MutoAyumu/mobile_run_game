public interface IState
{
    public void OnEnter();
    public int OnUpdate();
    public void OnExit();
}