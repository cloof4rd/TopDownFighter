using System.Collections.Generic;

namespace YogiGameCore.Utils.CommandPattern
{
    /// <summary>
    /// 指令代理 用于实现撤销重做
    /// 设计模式之命令模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandBroker<T> where T : class
    {
        private T source;
        private List<ICommand<T>> commands;
        private int index;
        private int maxCommandCount;

        public CommandBroker(T source, int maxCommandCount = 1000)
        {
            this.source = source;
            this.commands = new();
            this.index = -1;
            this.maxCommandCount = maxCommandCount;
        }

        public void SwitchSource(T source)
        {
            this.source = source;
        }

        public void Execute(ICommand<T> command)
        {
            if (index >= maxCommandCount - 1)
            {
                commands.RemoveAt(0);
                index--;
            }

            commands.RemoveRange(index + 1, commands.Count - index - 1);
            commands.Add(command);
            index++;
            command.Execute(source);
        }

        public void Undo()
        {
            if (index < 0)
                return;
            commands[index].Undo(source);
            index--;
        }

        public void Redo()
        {
            if (index >= commands.Count - 1)
                return;
            index++;
            commands[index].Execute(source);
        }

        public void Clear()
        {
            commands.Clear();
            index = -1;
        }
    }
}