using UnityEngine;
using Debug = UnityEngine.Debug;

namespace YogiGameCore.Log
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public static class LogCore
    {
        //TODO: LogLevel
        //TODO: 控制台
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        public static void Log(object message, Object content)
        {
            Debug.Log(message, content);
        }

        public static void Warning(object message, Object content)
        {
            Debug.LogWarning(message, content);
        }

        public static void Warning(object msg)
        {
            Debug.LogWarning(msg);
        }
        // public static void Error(object message)
        // {
        //     Debug.LogError(message);
        // }

        public static void Error(object message, Object content)
        {
            Debug.LogError(message, content);
        }

        public static void Error(object msg)
        {
            Debug.LogError(msg);
        }

        public static void LogInfo(this object obj)
        {
            Log(obj.ToString());
        }
        public static void LogWarning(this object obj)
        {
            Warning(obj.ToString());
        }
        public static void LogError(this object obj)
        {
            Error(obj.ToString());
        }
    }
}