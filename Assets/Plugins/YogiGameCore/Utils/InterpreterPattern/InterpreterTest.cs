/*
 * 解释器模式
 * 用于分析String为指令
 */

using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace YogiGameCore.Utils.InterpreterPattern
{
    public class InterpreterTest
    {
        public class ActionResult
        {
            public MethodInfo MethodInfo;
            public object[] Parameters;
        }

        public class GameMethods
        {
            public static int player0HP, player1HP;

            public static void AddHP(int playerIndex, int modifyHP)
            {
                //player add hp ...
                switch (playerIndex)
                {
                    case 0:
                        player0HP += modifyHP;
                        break;
                    case 1:
                        player1HP += modifyHP;
                        break;
                    default:
                        throw new System.Exception("playerIndex error");
                }
            }

            public static void AddItem(int playerIndex, int itemID)
            {
                //player add item ...
            }

            public static void WinGame(float cost)
            {
                //player win game ...
            }

            public static int GetHP(int playerIndex)
            {
                switch (playerIndex)
                {
                    case 0:
                        return player0HP;
                    case 1:
                        return player1HP;
                    default:
                        throw new System.Exception("playerIndex error");
                }
            }
        }

        public class InterpreterHandler
        {
            private Dictionary<string, MethodInfo> methodInfos;
            private const string FLEX = ";";

            public InterpreterHandler()
            {
                methodInfos = new Dictionary<string, MethodInfo>();

                var type = typeof(GameMethods);
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);
                foreach (var method in methods)
                {
                    methodInfos.Add(method.Name, method);
                }
            }

            public List<ActionResult> Decompilation(string expression)
            {
                var commanders = expression.Split(FLEX);
                List<ActionResult> results = new List<ActionResult>();
                foreach (var commander in commanders)
                {
                    ActionTerminalExpression actionTerminalExpression = new ActionTerminalExpression(methodInfos);
                    ActionResult actionResultOne = actionTerminalExpression.Interpret(commander);
                    if (actionResultOne != null)
                        results.Add(actionResultOne);
                }

                return results;
            }

            public string Compilation(List<ActionResult> results)
            {
                StringBuilder expression = new StringBuilder();
                for (var i = 0; i < results.Count; i++)
                {
                    if (i != 0)
                    {
                        expression.Append($"{FLEX}");
                    }

                    var result = results[i];
                    expression.Append($"{result.MethodInfo.Name}");
                    for (var j = 0; j < result.Parameters.Length; j++)
                    {
                        expression.Append(" ");
                        var parameter = result.Parameters[j];
                        expression.Append($"{parameter}");
                    }
                }

                return expression.ToString();
            }
        }

        /// <summary>
        /// 行为解析器
        /// </summary>
        public class ActionTerminalExpression : IExpression<string, ActionResult>
        {
            private Dictionary<string, MethodInfo> m_Dic;
            private Stack<string> m_PropertyStack;

            public ActionTerminalExpression(Dictionary<string, MethodInfo> dic)
            {
                this.m_Dic = dic;
                this.m_PropertyStack = new Stack<string>();
            }

            public ActionResult Interpret(string context)
            {
                if (context.IsNullOrEmpty())
                    return null;
                //Example:AddHP 0 GetHP 0
                ActionResult actionResults = new ActionResult();
                var orders = context.Split(' ');
                var methodName = orders[0];
                if (m_Dic.TryGetValue(methodName, out var value))
                {
                    actionResults.MethodInfo = value;
                }

                var paraLength = actionResults.MethodInfo.GetParameters().Length;
                //... 把属性值转换成对应的类型
                var parameters = new object[paraLength];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo parameterInfo = (actionResults.MethodInfo.GetParameters()[i]);
                    var configVal = orders[i + 1];
                    if (parameterInfo.ParameterType.IsEnum)
                    {
                        parameters[i] = System.Enum.Parse(parameterInfo.ParameterType, configVal);
                    }
                    else
                    {
                        switch (parameterInfo.ParameterType)
                        {
                            case var type when type == typeof(int):
                                parameters[i] = int.Parse(configVal);
                                break;
                            case var type when type == typeof(float):
                                parameters[i] = float.Parse(configVal);
                                break;
                            case var type when type == typeof(string):
                                parameters[i] = configVal;
                                break;
                            default:
                                throw new System.Exception("不支持的类型");
                        }
                    }
                }

                actionResults.Parameters = parameters;

                return actionResults;
            }
        }

        public void Main()
        {
            // var context = "1 if(GraterThen GetHP 0 5){AddHP 0 100};2 AddItem 0 5;3 WinGame 0.5";
            // 条件 = 条件(值)>值or条件(值)
            // 时机 (条件&条件|条件) {行为,行为,行为};
            // var context = "AddHP 0 100;AddItem 0 5;WinGame 0.5";

            var context = "AddHP 0 GetHP 0";
            InterpreterHandler handler = new InterpreterHandler();
            var message = handler.Decompilation(context);
            UnityEngine.Debug.Log(message);

            var compilation = handler.Compilation(message);
            UnityEngine.Debug.Log(compilation);
            var b = compilation == context;
            UnityEngine.Debug.Log(b);
        }
    }
}