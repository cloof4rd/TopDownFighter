namespace YogiGameCore.Utils.CommandPattern
{
    /// <summary>
    /// 指令接口 用于实现撤销重做
    /// 设计模式之命令模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommand<in T> where T : class
    {
        void Execute(T source);
        void Undo(T source);
    }
}