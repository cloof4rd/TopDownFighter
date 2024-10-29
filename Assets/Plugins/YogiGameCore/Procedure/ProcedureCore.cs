using System;
using YogiGameCore.FSM;

namespace YogiGameCore.Procedure
{
    /// <summary>
    /// 游戏流程核心 持有和管理所有流程
    /// </summary>
    public static class ProcedureCore
    {
        public class ProcedureFSMSystem : FSMSystem
        {
        }

        private static ProcedureFSMSystem _ProcedureFSMSystem;

        /// <summary>
        /// 初始化流程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Init<T>() where T : ProcedureStateBase
        {
            _ProcedureFSMSystem = new ProcedureFSMSystem();
            _ProcedureFSMSystem.Init<T, ProcedureStateBase>();
        }

        /// <summary>
        /// 切换不同流程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ChangeProcedure<T>() where T : ProcedureStateBase
        {
            _ProcedureFSMSystem.Change<T>();
        }

        public static void ChangeProcedure(Type nextState)
        {
            _ProcedureFSMSystem.Change(nextState);
        }
    }
}