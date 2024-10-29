using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using YogiGameCore.Log;
using YogiGameCore.Utils;

namespace YogiGameCore.FSM
{
    /// <summary>
    /// 状态机的核心控制类
    /// </summary>
    public class FSMSystem : IFSMSystemBase
    {
        private FSMState m_CurrentFsmState;
        protected Dictionary<Type, FSMState> _allFsmStates;

        /// <summary>
        /// 初始化FSM的所有状态(反射获得所有继承T2的子类作为状态机的状态) 并且设置初始状态机状态为T
        /// </summary>
        /// <typeparam name="T">将会把次类型作为初始节点(此类也需要继承T2)</typeparam>
        /// <typeparam name="T2">将会把所有继承此类的类作为状态子节点加入状态机</typeparam>
        public void Init<T, T2>()
            where T : FSMState
            where T2 : FSMState
        {
            // 反射获得所有继承FSMBase的类实例
            var fsmState = GetSubClassNames(typeof(T).Assembly, typeof(T2));
            _allFsmStates = new Dictionary<Type, FSMState>();
            for (var i = 0; i < fsmState.Count; i++)
            {
                var constructor = fsmState[i].GetConstructor(System.Type.EmptyTypes);
                var baseData = (T2)constructor.Invoke(null);
                _allFsmStates.Add(fsmState[i], baseData);
            }

            // 初始化最初的状态
            if (!_allFsmStates.TryGetValue(typeof(T), out m_CurrentFsmState))
            {
                Debug.LogError($"FSM Init Fail Type:{typeof(T)},{typeof(T2)}");
            }

            m_CurrentFsmState.OnEnter();
            // 每帧调用对于状态事件
            MonoEvent.Instance.UPDATE += Tick;
        }

        private void Tick()
        {
            m_CurrentFsmState.OnUpdate();
        }

        /// <summary>
        /// 切换不同状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Change<T>(bool isForceChange = true) where T : IFSMStateBase
        {
            if (!isForceChange && m_CurrentFsmState == _allFsmStates[typeof(T)])
                return;
            m_CurrentFsmState.OnExit();
            m_CurrentFsmState = _allFsmStates[typeof(T)];
            m_CurrentFsmState.OnEnter();
        }

        public void Change(Type nextStateType)
        {
            if (!_allFsmStates.ContainsKey(nextStateType))
                LogCore.Error($"FSM Change Fail Type:{nextStateType}");
            
            m_CurrentFsmState.OnExit();
            m_CurrentFsmState = _allFsmStates[nextStateType];
            m_CurrentFsmState.OnEnter();
        }


        /// <summary>
        /// C#获取一个类在其所在的程序集中的所有子类
        /// </summary>
        /// <param name="parentType">给定的类型</param>
        /// <returns>所有子类类型</returns>
        private List<Type> GetSubClassNames(Assembly assembly, Type parentType)
        {
            var subTypeList = new List<Type>();
            //var assembly = parentType.Assembly; //获取当前父类所在的程序集``
            var assemblyAllTypes = assembly.GetTypes(); //获取该程序集中的所有类型
            foreach (var itemType in assemblyAllTypes) //遍历所有类型进行查找
            {
                var baseType = itemType.BaseType; //获取元素类型的基类
                if (baseType != null) //如果有基类
                {
                    if (baseType.Name == parentType.Name) //如果基类就是给定的父类
                    {
                        subTypeList.Add(itemType); //加入子类表中
                    }
                }
            }

            return subTypeList; //获取所有子类类型的名称
        }
    }
}