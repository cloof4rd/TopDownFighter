namespace YogiGameCore.FSM
{
    public interface IFSMStateBase
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}