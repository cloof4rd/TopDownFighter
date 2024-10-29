namespace YogiGameCore.Utils.InterpreterPattern
{
    /// <summary>
    /// 解释器
    /// </summary>
    public interface IExpression<in T1, out T2>
    {
        T2 Interpret(T1 context);
    }
}