using UnityEngine;
using System;

//便利能力:
//1.使用的时候提升复用性不用每次都打单例模式
//2.继承就行了,使用方便
//使用方法:
//1. 继承        Singleton<类名> 就可以获得单例能力
//2. MonoEvent.UPDATE += 方法名 就可以把方法放在Update中执行 同理其他亦然
//3. 继承        MonoSingleton<类名> 就可以获得继承Mono的单例模式 !!不推荐直接再场景中拖放这个东西
//注: 第3点 调用前 先确定isGlobal 是否为全局类 默认不是全局类

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 单例 不继承mono
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private static T _instance;
        private static readonly object objlock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objlock)
                    {
                        if (_instance == null)
                            _instance = new T();
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// 不用继承Mono,但是想要用生命周期做些事情
    /// </summary>
    public class MonoEvent : MonoSingleton<MonoEvent>
    {
        public event Action UPDATE;
        public event Action FIXEDUPDATE;
        public event Action ONGUI;
        public event Action LATEUPDATE;

        private void Update()
        {
            UPDATE?.Invoke();
        }

        private void FixedUpdate()
        {
            FIXEDUPDATE?.Invoke();
        }

        private void OnGUI()
        {
            ONGUI?.Invoke();
        }

        private void LateUpdate()
        {
            LATEUPDATE?.Invoke();
        }
    }

    /// <summary>
    /// 继承mono的单例模式 , 不建议手动在Scene中的某个GameObject上挂载继承Mono的单例类，调用的时候代码会自动创建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static readonly object _lock = new object();

        protected static bool ApplicationIsQuitting { get; private set; }

        /// <summary>
        /// 是否是全局变量
        /// </summary>
        protected static bool isGolbal = false;

        static MonoSingleton()
        {
            ApplicationIsQuitting = false;
        }

        /// <summary>
        /// 单例 如果在程序中找不到那么就实例化 如果是全局变量就不会被DestroyOnLoad
        /// </summary>
        public static T Instance
        {
            get
            {
                if (ApplicationIsQuitting)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.Log("[singleton]" + typeof(T) + "already destroy on application quit." +
                                         "Won't create again - returning null.");
                    }

                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (FindObjectsOfType<T>().Length > 1)
                        {
                            if (Debug.isDebugBuild)
                            {
                                Debug.LogWarning("[Singleton]" + typeof(T).Name +
                                                 " should never be more than 1 in scene!");
                            }

                            return _instance;
                        }
                    }

                    if (_instance == null)
                    {
                        GameObject singletonObj = new GameObject();
                        _instance = singletonObj.AddComponent<T>();
                        singletonObj.name = "(singleton)" + typeof(T).Name;
                        if (isGolbal && Application.isPlaying)
                        {
                            DontDestroyOnLoad(singletonObj);
                        }

                        return _instance;
                    }
                }

                return _instance;
            }
        }

        public void OnApplicationQuit()
        {
            ApplicationIsQuitting = true;
        }
    }
}