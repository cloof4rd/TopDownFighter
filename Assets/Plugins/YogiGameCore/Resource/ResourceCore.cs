using Cysharp.Threading.Tasks;
using UnityEngine;
using YogiGameCore.Const;
using YogiGameCore.Log;
using Object = UnityEngine.Object;

namespace YogiGameCore.Resource
{
    public static class ResourceCore
    {
        /// <summary>
        /// 常用资源配置路径
        /// </summary>
        public static GameResourceConfig PathConfig
        {
            get
            {
                if (_PathConfig == null)
                {
                    _PathConfig = Resources.Load<GameResourceConfig>("Configs/GamePathConfig");
                }

                if (_PathConfig == null)
                {
                    Debug.LogError(
                        "Need Create GameResourceConfig Type File To Path \"Resources/Configs/GamePathConfig.asset\"");
                }

                return _PathConfig;
            }
        }

        private static GameResourceConfig _PathConfig;

        /// <summary>
        /// 异步加载UI面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static UniTask<T> LoadUIPanel<T>() where T : Object
        {
            var path = $"{PathConfig.UIPanelPath}/{typeof(T).Name}";
            return Load<T>(path);
        }

        public static UniTask<T> LoadSingleUI<T>() where T : Object
        {
            var path = $"{PathConfig.SingleUIPath}/{typeof(T).Name}";
            return Load<T>(path);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ResourceTypeErrorException"></exception>
        /// <exception cref="ResourcePathErrorException"></exception>
        public static async UniTask<T> Load<T>(string path) where T : Object
        {
            LogCore.Log($"Load:{path}");
            var assets = await Resources.LoadAsync<T>(path);
            if (assets != null)
            {
                var data = assets as T;
                if (data != null)
                {
                    return data;
                }
                else
                {
                    throw new ResourceTypeErrorException();
                }
            }
            else
            {
                LogCore.Error($"Load Error Path:{path}");
                return null;
                // throw new ResourcePathErrorException(path);
            }
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] LoadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }
    }
}