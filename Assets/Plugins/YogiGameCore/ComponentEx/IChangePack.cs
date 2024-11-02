namespace YogiGameCore.ComponentEx
{
    interface IChangePack
    {
        public void SetEnable(bool isEnable);
        public void Init(UIListener ImageBtnListener);
        public void SetState(ChangeState index);
        public void SetHold(bool value);
    }

    public enum ChangeState
    {
        Normal = 0,
        Highlight,
        Pressed,
        Selected,
        Disabled,
        Hold
    }
}