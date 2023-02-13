public interface IState
{
    public void Init();
    public void OnEnter();
    public int OnUpdate();
    public void OnExit();
}