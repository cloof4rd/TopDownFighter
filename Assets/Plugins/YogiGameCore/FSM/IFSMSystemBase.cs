namespace YogiGameCore.FSM
{
    public interface IFSMSystemBase
    {
        void Change<T>(bool isForceChange = true) where T : IFSMStateBase;
    }
}