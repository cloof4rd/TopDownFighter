namespace YogiGameCore.FSM
{
    /// <summary>
    /// 任何状态机的状态节点继承这个类
    /// </summary>
    public abstract class FSMState : IFSMStateBase
    {
        public virtual void OnEnter(){}
        public virtual void OnUpdate(){}
        public virtual void OnExit(){}
    }
}