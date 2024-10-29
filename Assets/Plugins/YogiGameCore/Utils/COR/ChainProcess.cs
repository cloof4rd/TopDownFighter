using System;
using System.Collections.Generic;

namespace YogiGameCore.Utils.COR
{
    /// <summary>
    /// 职责链处理器, 可以继承此类然后重写出入库方法,来对数据处理前后进行处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChainProcess<T> : IAssetsModify<T> where T : IContent
    {
        public delegate bool FilterFunc(T content);

        public delegate bool FilterFuncNoInput();

        public delegate void LogicFunc(T content);

        public delegate void LogicFuncNoInput();

        private T Content;
        private readonly List<FilterExpression> Filters = new List<FilterExpression>();
        private readonly List<LogicExpression> Logics = new List<LogicExpression>();
        private FilterOperator m_FirstFilterOperator;

        /// <summary>
        /// 职责链传入对应处理数据
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ChainProcess<T> AddContent(T content)
        {
            Content = content;
            HandleAssetIn(Content);
            return this;
        }

        #region FilterFunc

        /// <summary>
        /// 第一次添加条件判断(可以直接使用or或者and,默认执行true and Filter)
        /// </summary>
        /// <param name="filterFunc"></param>
        /// <returns></returns>
        public ChainProcess<T> IF(FilterFunc filterFunc)
        {
            return And(filterFunc);
        }

        /// <summary>
        /// 添加and条件
        /// </summary>
        /// <param name="filterFunc"></param>
        /// <returns></returns>
        public ChainProcess<T> And(FilterFunc filterFunc)
        {
            return AddFilter(FilterOperator.And, filterFunc);
        }

        /// <summary>
        /// 添加or条件
        /// </summary>
        /// <param name="filterFunc"></param>
        /// <returns></returns>
        public ChainProcess<T> Or(FilterFunc filterFunc)
        {
            return AddFilter(FilterOperator.Or, filterFunc);
        }

        /// <summary>
        /// 添加条件过滤器
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <param name="filterFunc"></param>
        /// <returns></returns>
        private ChainProcess<T> AddFilter(FilterOperator filterOperator, FilterFunc filterFunc)
        {
            if (m_FirstFilterOperator == FilterOperator.None)
            {
                m_FirstFilterOperator = FilterOperator.And;
            }

            Filters.Add(new FilterExpression() { FilterOperator = filterOperator, FilterFunc = filterFunc });

            return this;
        }

        #endregion

        #region FilterFuncNoInput

        /// <summary>
        /// 第一次添加条件判断(可以直接使用or或者and,默认执行true and Filter)
        /// </summary>
        /// <param name="filterFuncNoInput"></param>
        /// <returns></returns>
        public ChainProcess<T> IF(FilterFuncNoInput filterFuncNoInput)
        {
            return And(filterFuncNoInput);
        }

        /// <summary>
        /// 添加and条件
        /// </summary>
        /// <param name="filterFuncNoInput"></param>
        /// <returns></returns>
        public ChainProcess<T> And(FilterFuncNoInput filterFuncNoInput)
        {
            return AddFilter(FilterOperator.And, filterFuncNoInput);
        }

        /// <summary>
        /// 添加or条件
        /// </summary>
        /// <param name="filterFuncNoInput"></param>
        /// <returns></returns>
        public ChainProcess<T> Or(FilterFuncNoInput filterFuncNoInput)
        {
            return AddFilter(FilterOperator.Or, filterFuncNoInput);
        }

        /// <summary>
        /// 添加条件过滤器
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <param name="filterFuncNoInput"></param>
        /// <returns></returns>
        private ChainProcess<T> AddFilter(FilterOperator filterOperator, FilterFuncNoInput filterFuncNoInput)
        {
            if (m_FirstFilterOperator == FilterOperator.None)
            {
                m_FirstFilterOperator = FilterOperator.And;
            }

            Filters.Add(new FilterExpression()
                { FilterOperator = filterOperator, FilterFuncNoInput = filterFuncNoInput });

            return this;
        }

        #endregion


        /// <summary>
        /// 添加逻辑
        /// </summary>
        /// <param name="logicFunc"></param>
        /// <returns></returns>
        public ChainProcess<T> AddLogic(LogicFunc logicFunc)
        {
            Logics.Add(new LogicExpression() { LogicFunc = logicFunc });
            return this;
        }

        /// <summary>
        /// 添加逻辑
        /// </summary>
        /// <param name="logicFuncNoInput"></param>
        /// <returns></returns>
        public ChainProcess<T> AddLogic(LogicFuncNoInput logicFuncNoInput)
        {
            Logics.Add(new LogicExpression(){LogicFuncNoInput = logicFuncNoInput});
            return this;
        }

        /// <summary>
        /// 执行职责链
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool Process()
        {
            bool result = true;
            for (var i = 0; i < Filters.Count; i++)
            {
                var filterExpression = Filters[i];
                result = filterExpression.Invoke(Content, result);
            }

            if (result)
            {
                foreach (var logicFunc in Logics)
                {
                    logicFunc.Invoke(Content);
                }
            }

            HandleAssetOut(Content);
            return result;
        }

        public virtual void HandleAssetIn(T content)
        {
        }

        public virtual void HandleAssetOut(T content)
        {
        }

        private enum FilterOperator
        {
            None,
            And,
            Or
        }

        private class FilterExpression
        {
            public FilterOperator FilterOperator;
            public FilterFunc FilterFunc;
            public FilterFuncNoInput FilterFuncNoInput;

            public bool Invoke(T content, bool inputVal)
            {
                switch (FilterOperator)
                {
                    case FilterOperator.And:
                        if (FilterFunc != null)
                        {
                            inputVal &= FilterFunc.Invoke(content);
                        }
                        else if (FilterFuncNoInput != null)
                        {
                            inputVal &= FilterFuncNoInput.Invoke();
                        }

                        break;
                    case FilterOperator.Or:
                        if (FilterFunc != null)
                        {
                            inputVal |= FilterFunc.Invoke(content);
                        }
                        else if (FilterFuncNoInput != null)
                        {
                            inputVal |= FilterFuncNoInput.Invoke();
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return inputVal;
            }
        }

        private class LogicExpression
        {
            public LogicFunc LogicFunc;
            public LogicFuncNoInput LogicFuncNoInput;

            public void Invoke(T content)
            {
                LogicFunc?.Invoke(content);
                LogicFuncNoInput?.Invoke();
            }
        }
    }
}