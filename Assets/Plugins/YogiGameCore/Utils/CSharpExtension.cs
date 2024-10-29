using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YogiGameCore.Log;
using Object = UnityEngine.Object;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 一些基础类型的扩展
    /// </summary>
    public static class BasicValueExtension
    {
        static void Example()
        {
            int a = 2, b = 5;
            if (a.Is(b))
                return;
            Func<int, bool> f;
            f = (int value) => { return true; };
            if (a.Is((int v) => { return v == 3; }))
                return;
            (a == 2).Do("test".LogInfo);
            (b == 5).Do((v) =>
            {
                if (v)
                    $"Do if {v}".LogInfo();
                else
                    $"Dont Do {v}".LogInfo();
            });

            // 样例代码逻辑
            bool hasEat = false;
            bool hasFood = false;

            var result = hasEat.DoIfNot(Eat)
                .DoIfNot(FoodPrepare)
                .Do(Eat);
            $"操作是否成功{result}".LogInfo();

            void Eat()
            {
                hasFood.Do(() => "Eat".LogInfo())
                    .DoIfNot("Not Enough Food".LogWarning);
            }

            void FoodPrepare()
            {
                hasFood
                    .DoIfNot(() => hasFood = true);
            }

            // 异步测试
            (1 == 1).Do(TestAsync);

            async UniTask TestAsync()
            {
                await UniTask.Yield();
                "TestAsync".LogInfo();
            }
        }


        /// <summary>
        /// 是否相等
        /// 
        /// 示例：
        /// <code>
        /// if (this.Is(player))
        /// {
        ///     ...
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Is(this object selfObj, object value)
        {
            return selfObj == value;
        }

        public static bool Is<T>(this T selfObj, Func<T, bool> condition)
        {
            return condition(selfObj);
        }

        /// <summary>
        /// 表达式成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Do(this bool selfCondition, Action action)
        {
            if (selfCondition)
            {
                action();
            }

            return selfCondition;
        }

        /// <summary>
        /// 不管表达成不成立 都执行 Action，并把结果返回
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do((result)=>Debug.Log("1 == 1:" + result);
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool Do(this bool selfCondition, Action<bool> action)
        {
            action(selfCondition);
            return selfCondition;
        }

        /// <summary>
        /// 表达式不成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool DoIfNot(this bool selfCondition, Action action)
        {
            if (!selfCondition)
            {
                action();
            }

            return selfCondition;
        }

        /// <summary>
        /// (异步)表达式成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async UniTask<bool> Do(this bool selfCondition, Func<UniTask> action)
        {
            if (selfCondition)
            {
                await action.Invoke();
            }

            return selfCondition;
        }

        /// <summary>
        /// (异步)不管表达成不成立 都执行 Action，并把结果返回
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async UniTask<bool> Do(this bool selfCondition, Func<bool, UniTask> action)
        {
            await action.Invoke(selfCondition);
            return selfCondition;
        }

        /// <summary>
        /// (异步)表达式不成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async UniTask<bool> DoIfNot(this bool selfCondition, Func<UniTask> action)
        {
            if (!selfCondition)
            {
                await action.Invoke();
            }

            return selfCondition;
        }

        /// <summary>
        /// (异步)表达式成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async UniTask<bool> Do(this UniTask<bool> selfCondition, Func<UniTask> action)
        {
            var condition = await selfCondition;
            if (condition)
            {
                await action.Invoke();
            }

            return condition;
        }

        /// <summary>
        /// (异步)表达式不成立 则执行 Action
        /// 
        /// 示例:
        /// <code>
        /// (1 == 1).Do(()=>Debug.Log("1 == 1");
        /// </code>
        /// </summary>
        /// <param name="selfCondition"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async UniTask<bool> DoIfNot(this UniTask<bool> selfCondition, Func<UniTask> action)
        {
            var condition = await selfCondition;
            if (!condition)
            {
                await action.Invoke();
            }

            return condition;
        }
    }

    /// <summary>
    /// 通用的扩展，类的扩展
    /// </summary>
    public static class ClassExtention
    {
        /// <summary>
        /// 功能：判断是否为空
        /// 
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNull()) // 等价于 simpleObject == null
        /// {
        ///     // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象(this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否为空</returns>
        public static bool IsNull<T>(this T selfObj) where T : class
        {
            return null == selfObj;
        }

        /// <summary>
        /// 功能：判断不是为空
        /// 示例：
        /// <code>
        /// var simpleObject = new object();
        ///
        /// if (simpleObject.IsNotNull()) // 等价于 simpleObject != null
        /// {
        ///    // do sth
        /// }
        /// </code>
        /// </summary>
        /// <param name="selfObj">判断对象（this)</param>
        /// <typeparam name="T">对象的类型（可不填）</typeparam>
        /// <returns>是否不为空</returns>
        public static bool IsNotNull<T>(this T selfObj) where T : class
        {
            return null != selfObj;
        }

        /// <summary>
        /// 功能: 如果selfObject不为空时,传入对象并执行方法逻辑
        /// </summary>
        /// <param name="selfObj"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void DoIfNotNull<T>(this T selfObj, Action<T> action) where T : class
        {
            if (selfObj != null)
            {
                action(selfObj);
            }
        }
    }

    /// <summary>
    /// Func、Action、delegate 的扩展
    /// </summary>
    public static class FuncOrActionOrEventExtension
    {
        #region Func Extension

        /// <summary>
        /// 功能：不为空则调用 Func
        /// 
        /// 示例:
        /// <code>
        /// Func<int> func = ()=> 1;
        /// var number = func.InvokeGracefully(); // 等价于 if (func != null) number = func();
        /// </code>
        /// </summary>
        /// <param name="selfFunc"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T InvokeGracefully<T>(this Func<T> selfFunc)
        {
            return null != selfFunc ? selfFunc() : default(T);
        }

        #endregion

        #region Action

        /// <summary>
        /// 功能：不为空则调用 Action
        /// 
        /// 示例:
        /// <code>
        /// System.Action action = () => Debug.Log("action called");
        /// action.InvokeGracefully(); // if (action != null) action();
        /// </code>
        /// </summary>
        /// <param name="selfAction"> action 对象 </param>
        /// <returns> 是否调用成功 </returns>
        public static bool InvokeGracefully(this Action selfAction)
        {
            if (null != selfAction)
            {
                selfAction();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 不为空则调用 Action<T>
        /// 
        /// 示例:
        /// <code>
        /// System.Action<int> action = (number) => Debug.Log("action called" + number);
        /// action.InvokeGracefully(10); // if (action != null) action(10);
        /// </code>
        /// </summary>
        /// <param name="selfAction"> action 对象</param>
        /// <typeparam name="T">参数</typeparam>
        /// <returns> 是否调用成功</returns>
        public static bool InvokeGracefully<T>(this Action<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                selfAction(t);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 不为空则调用 Action<T,K>
        ///
        /// 示例
        /// <code>
        /// System.Action<int,string> action = (number,name) => Debug.Log("action called" + number + name);
        /// action.InvokeGracefully(10,"qframework"); // if (action != null) action(10,"qframework");
        /// </code>
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully<T, K>(this Action<T, K> selfAction, T t, K k)
        {
            if (null != selfAction)
            {
                selfAction(t, k);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 不为空则调用委托
        ///
        /// 示例：
        /// <code>
        /// // delegate
        /// TestDelegate testDelegate = () => { };
        /// testDelegate.InvokeGracefully();
        /// </code>
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call suceed </returns>
        public static bool InvokeGracefully(this Delegate selfAction, params object[] args)
        {
            if (null != selfAction)
            {
                selfAction.DynamicInvoke(args);
                return true;
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// 泛型工具
    /// 
    /// 实例：
    /// <code>
    /// 示例：
    /// var typeName = GenericUtil.GetTypeName<string>();
    /// typeName.LogInfo(); // string
    /// </code>
    /// </summary>
    public static class GenericUtil
    {
        /// <summary>
        /// 获取泛型名字
        /// <code>
        /// var typeName = GenericUtil.GetTypeName<string>();
        /// typeName.LogInfo(); // string
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTypeName<T>()
        {
            return typeof(T).ToString();
        }
    }

    /// <summary>
    /// 可枚举的集合扩展（Array、List<T>、Dictionary<K,V>)
    /// </summary>
    public static class IEnumerableExtension
    {
        #region Array Extension

        /// <summary>
        /// 遍历数组
        /// <code>
        /// var testArray = new[] { 1, 2, 3 };
        /// testArray.ForEach(number => number.LogInfo());
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns> 返回自己 </returns>
        public static T[] ForEach<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        /// <summary>
        /// 遍历 IEnumerable
        /// <code>
        /// // IEnumerable<T>
        /// IEnumerable<int> testIenumerable = new List<int> { 1, 2, 3 };
        /// testIenumerable.ForEach(number => number.LogInfo());
        /// // 支持字典的遍历
        /// new Dictionary<string, string>()
        ///         .ForEach(keyValue => Debug.Log("key:{0},value:{1}", keyValue.Key, keyValue.Value));
        /// </code>
        /// </summary>
        /// <returns>The each.</returns>
        /// <param name="selfArray">Self array.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (T item in selfArray)
            {
                action(item);
            }

            return selfArray;
        }

        #endregion

        #region List Extension

        /// <summary>
        /// 倒序遍历
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse(number => number.LogInfo()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>返回自己</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();

            for (int i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i]);

            return selfList;
        }

        /// <summary>
        /// 倒序遍历（可获得索引)
        /// <code>
        /// var testList = new List<int> { 1, 2, 3 };
        /// testList.ForEachReverse((number,index)=> number.LogInfo()); // 3, 2, 1
        /// </code>
        /// </summary>
        /// <returns>The each reverse.</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse<T>(this List<T> selfList, Action<T, int> action)
        {
            if (action == null) throw new ArgumentException();

            for (int i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i], i);

            return selfList;
        }

        /// <summary>
        /// 遍历列表(可获得索引）
        /// <code>
        /// var testList = new List<int> {1, 2, 3 };
        /// testList.Foreach((number,index)=>number.LogInfo()); // 1, 2, 3,
        /// </code>
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="list">目标表</param>
        /// <param name="action">行为</param>
        public static void ForEach<T>(this List<T> list, Action<int, T> action)
        {
            for (int i = 0; i < list.Count; i++)
            {
                action(i, list[i]);
            }
        }

        #endregion

        #region Dictionary Extension

        /// <summary>
        /// 合并字典
        /// <code>
        /// // 示例
        /// var dictionary1 = new Dictionary<string, string> { { "1", "2" } };
        /// var dictionary2 = new Dictionary<string, string> { { "3", "4" } };
        /// var dictionary3 = dictionary1.Merge(dictionary2);
        /// dictionary3.ForEach(pair => Debug.Log("{0}:{1}", pair.Key, pair.Value));
        /// </code>
        /// </summary>
        /// <returns>The merge.</returns>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="dictionaries">Dictionaries.</param>
        /// <typeparam name="TKey">The 1st type parameter.</typeparam>
        /// <typeparam name="TValue">The 2nd type parameter.</typeparam>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            params Dictionary<TKey, TValue>[] dictionaries)
        {
            return dictionaries.Aggregate(dictionary,
                (current, dict) => current.Union(dict).ToDictionary(kv => kv.Key, kv => kv.Value));
        }

        /// <summary>
        /// 遍历字典
        /// <code>
        /// var dict = new Dictionary<string,string> {{"name","liangxie},{"age","18"}};
        /// dict.ForEach((key,value)=> Debug.Log("{0}:{1}",key,value);//  name:liangxie    age:18
        /// </code>
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        public static void ForEach<K, V>(this Dictionary<K, V> dict, Action<K, V> action)
        {
            Dictionary<K, V>.Enumerator dictE = dict.GetEnumerator();

            while (dictE.MoveNext())
            {
                KeyValuePair<K, V> current = dictE.Current;
                action(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        /// <summary>
        /// 字典添加新的词典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        /// <param name="addInDict"></param>
        /// <param name="isOverride"></param>
        public static void AddRange<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> addInDict,
            bool isOverride = false)
        {
            Dictionary<K, V>.Enumerator dictE = addInDict.GetEnumerator();

            while (dictE.MoveNext())
            {
                KeyValuePair<K, V> current = dictE.Current;
                if (dict.ContainsKey(current.Key))
                {
                    if (isOverride)
                        dict[current.Key] = current.Value;
                    continue;
                }

                dict.Add(current.Key, current.Value);
            }

            dictE.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// 对 System.IO 的一些扩展
    /// </summary>
    public static class IOExtension
    {
        static void Exapmle()
        {
            string dir = Application.dataPath.CombinePath("Test");
            string path = dir.CombinePath("A.txt");
            string smartReadText = path.SmartReadText();
            Dictionary<string, string> smartReadDirTextFile =
                dir.SmartReadDirTextFile("*.txt", SearchOption.AllDirectories);
        }

        #region 文件

        public static Dictionary<string, string> SmartReadDirTextFile(this string dirFullPath,
            string searchPattern = "*.*",
            SearchOption option = SearchOption.AllDirectories)
        {
            return dirFullPath.CreateDirIfNotExists()
                .ReadAllDirTextFile(searchPattern, option);
        }

        public static Dictionary<string, string> ReadAllDirTextFile(this string dirFullPath,
            string searchPattern = "*.*",
            SearchOption option = SearchOption.AllDirectories)
        {
            var files = Directory.GetFiles(dirFullPath, searchPattern, option);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var file in files)
            {
                dict.Add(file, file.ReadAllText());
            }

            return dict;
        }

        public static string SmartReadText(this string filePath, string defaultText = "")
        {
            return filePath.CreateDirIfNotExists4FilePath()
                .CreateFileIfNotExists()
                .ReadAllText();
        }

        public static string CreateFileIfNotExists(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                var fileStream = File.Create(filePath);
                fileStream.Close();
            }

            return filePath;
        }

        public static string ReadAllText(this string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public static void SmartWriteText(this string filePath, string content, Encoding encoding)
        {
            filePath.CreateDirIfNotExists4FilePath()
                .CreateFileIfNotExists()
                .WriteAllText(content, encoding);
        }

        public static void WriteAllText(this string filePath, string content, Encoding encoding)
        {
            File.WriteAllText(filePath, content, encoding);
        }

        #endregion

        #region 目录

        /// <summary>
        /// 检测路径是否存在，如果不存在则创建
        /// </summary>
        /// <param name="path"></param>
        public static string CreateDirIfNotExists4FilePath(this string path)
        {
            string direct = Path.GetDirectoryName(path);

            if (!Directory.Exists(direct))
            {
                Directory.CreateDirectory(direct);
            }

            return path;
        }

        /// <summary>
        /// 创建新的文件夹,如果存在则不创建
        /// <code>
        /// var testDir = "Assets/TestFolder";
        /// testDir.CreateDirIfNotExists();
        /// // 结果为，在 Assets 目录下创建 TestFolder
        /// </code>
        /// </summary>
        public static string CreateDirIfNotExists(this string dirFullPath)
        {
            if (!Directory.Exists(dirFullPath))
            {
                Directory.CreateDirectory(dirFullPath);
            }

            return dirFullPath;
        }

        /// <summary>
        /// 删除文件夹，如果存在
        /// <code>
        /// var testDir = "Assets/TestFolder";
        /// testDir.DeleteDirIfExists();
        /// // 结果为，在 Assets 目录下删除了 TestFolder
        /// </code>
        /// </summary>
        public static void DeleteDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }
        }

        /// <summary>
        /// 清空 Dir（保留目录),如果存在。
        /// <code>
        /// var testDir = "Assets/TestFolder";
        /// testDir.EmptyDirIfExists();
        /// // 结果为，清空了 TestFolder 里的内容
        /// </code>
        /// </summary>
        public static void EmptyDirIfExists(this string dirFullPath)
        {
            if (Directory.Exists(dirFullPath))
            {
                Directory.Delete(dirFullPath, true);
            }

            Directory.CreateDirectory(dirFullPath);
        }

        /// <summary>
        /// 删除文件 如果存在
        /// <code>
        /// // 示例
        /// var filePath = "Assets/Test.txt";
        /// File.Create("Assets/Test);
        /// filePath.DeleteFileIfExists();
        /// // 结果为，删除了 Test.txt
        /// </code>
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns> 是否进行了删除操作 </returns>
        public static bool DeleteFileIfExists(this string fileFullPath)
        {
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
                return true;
            }

            return false;
        }

        #endregion

        #region 路径

        /// <summary>
        /// 合并路径
        /// <code>
        /// // 示例：
        /// Application.dataPath.CombinePath("Resources").LogInfo();  // /projectPath/Assets/Resources
        /// </code>
        /// </summary>
        /// <param name="selfPath"></param>
        /// <param name="toCombinePath"></param>
        /// <returns> 合并后的路径 </returns>
        public static string CombinePath(this string selfPath, string toCombinePath)
        {
            return Path.Combine(selfPath, toCombinePath);
        }

        #endregion

        #region 未经过测试

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void OpenFolder(string path)
        {
#if UNITY_STANDALONE_OSX
            System.Diagnostics.Process.Start("open", path);
#elif UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", path);
#endif
        }


        /// <summary>
        /// 获取文件夹名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string fileName)
        {
            fileName = MakePathStandard(fileName);
            return fileName.Substring(0, fileName.LastIndexOf('/'));
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetFileName(string path, char separator = '/')
        {
            path = IOExtension.MakePathStandard(path);
            return path.Substring(path.LastIndexOf(separator) + 1);
        }

        /// <summary>
        /// 获取不带后缀的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string fileName, char separator = '/')
        {
            return GetFilePathWithoutExtension(GetFileName(fileName, separator));
        }

        /// <summary>
        /// 获取不带后缀的文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFilePathWithoutExtension(string fileName)
        {
            if (fileName.Contains("."))
                return fileName.Substring(0, fileName.LastIndexOf('.'));
            return fileName;
        }

        /// <summary>
        /// 使目录存在,Path可以是目录名必须是文件名
        /// </summary>
        /// <param name="path"></param>
        public static void MakeFileDirectoryExist(string path)
        {
            string root = Path.GetDirectoryName(path);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
        }

        /// <summary>
        /// 使目录存在
        /// </summary>
        /// <param name="path"></param>
        public static void MakeDirectoryExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 获取父文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPathParentFolder(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            return Path.GetDirectoryName(path);
        }


        /// <summary>
        /// 使路径标准化，去除空格并将所有'\'转换为'/'
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string MakePathStandard(string path)
        {
            return path.Trim().Replace("\\", "/");
        }

        public static List<string> GetDirSubFilePathList(this string dirABSPath, bool isRecursive = true,
            string suffix = "")
        {
            List<string> pathList = new List<string>();
            DirectoryInfo di = new DirectoryInfo(dirABSPath);

            if (!di.Exists)
            {
                return pathList;
            }

            FileInfo[] files = di.GetFiles();
            foreach (FileInfo fi in files)
            {
                if (!string.IsNullOrEmpty(suffix))
                {
                    if (!fi.FullName.EndsWith(suffix, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                }

                pathList.Add(fi.FullName);
            }

            if (isRecursive)
            {
                DirectoryInfo[] dirs = di.GetDirectories();
                foreach (DirectoryInfo d in dirs)
                {
                    pathList.AddRange(GetDirSubFilePathList(d.FullName, isRecursive, suffix));
                }
            }

            return pathList;
        }

        public static List<string> GetDirSubDirNameList(this string dirABSPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirABSPath);

            DirectoryInfo[] dirs = di.GetDirectories();

            return dirs.Select(d => d.Name).ToList();
        }

        public static string GetFileName(this string absOrAssetsPath)
        {
            string name = absOrAssetsPath.Replace("\\", "/");
            int lastIndex = name.LastIndexOf("/");

            return lastIndex >= 0 ? name.Substring(lastIndex + 1) : name;
        }

        public static string GetFileNameWithoutExtend(this string absOrAssetsPath)
        {
            string fileName = GetFileName(absOrAssetsPath);
            int lastIndex = fileName.LastIndexOf(".");

            return lastIndex >= 0 ? fileName.Substring(0, lastIndex) : fileName;
        }

        public static string GetFileExtendName(this string absOrAssetsPath)
        {
            int lastIndex = absOrAssetsPath.LastIndexOf(".");

            if (lastIndex >= 0)
            {
                return absOrAssetsPath.Substring(lastIndex);
            }

            return string.Empty;
        }

        public static string GetDirPath(this string absOrAssetsPath)
        {
            string name = absOrAssetsPath.Replace("\\", "/");
            int lastIndex = name.LastIndexOf("/");
            return name.Substring(0, lastIndex + 1);
        }

        public static string GetLastDirName(this string absOrAssetsPath)
        {
            string name = absOrAssetsPath.Replace("\\", "/");
            string[] dirs = name.Split('/');

            return absOrAssetsPath.EndsWith("/") ? dirs[dirs.Length - 2] : dirs[dirs.Length - 1];
        }

        #endregion
    }

    /// <summary>
    /// 简单的概率计算
    /// </summary>
    public static class ProbabilityUtil
    {
        /// <summary>
        /// 随机选择一堆数组中的一个数组
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomValueFrom<T>(params T[] values)
        {
            return values[UnityEngine.Random.Range(0, values.Length)];
        }

        /// <summary>
        /// 百分百概率
        /// </summary>
        /// <param name="percent"> 0 ~ 100 </param>
        /// <returns></returns>
        public static bool PercentProbability(int percent)
        {
            return UnityEngine.Random.Range(0, (float)percent) < percent / 2.0f;
        }
    }

    /// <summary>
    /// 面向对象扩展（继承、封装、多态)
    /// </summary>
    public static class OOPExtension
    {
        interface ExampleInterface
        {
        }

        public static void Example()
        {
            if (typeof(OOPExtension).ImplementsInterface<ExampleInterface>())
            {
            }

            if (new object().ImplementsInterface<ExampleInterface>())
            {
            }
        }


        /// <summary>
        /// Determines whether the type implements the specified interface
        /// and is not an interface itself.
        /// </summary>
        /// <returns><c>true</c>, if interface was implementsed, <c>false</c> otherwise.</returns>
        /// <param name="type">Type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool ImplementsInterface<T>(this Type type)
        {
            return !type.IsInterface && type.GetInterfaces().Contains(typeof(T));
        }

        /// <summary>
        /// Determines whether the type implements the specified interface
        /// and is not an interface itself.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return !type.IsInterface && type.GetInterfaces().Contains(interfaceType);
        }

        /// <summary>
        /// Determines whether the type implements the specified interface
        /// and is not an interface itself.
        /// </summary>
        /// <returns><c>true</c>, if interface was implementsed, <c>false</c> otherwise.</returns>
        /// <param name="type">Type.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static bool ImplementsInterface<T>(this object obj)
        {
            Type type = obj.GetType();
            return !type.IsInterface && type.GetInterfaces().Contains(typeof(T));
        }

        /// <summary>
        /// Determines whether the type implements the specified interface
        /// and is not an interface itself.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool ImplementsInterface(this object obj, Type interfaceType)
        {
            Type type = obj.GetType();
            return !type.IsInterface && type.GetInterfaces().Contains(interfaceType);
        }
    }

    /// <summary>
    /// 程序集工具
    /// </summary>
    public class AssemblyUtil
    {
        /// <summary>
        /// 获取 Assembly-CSharp 程序集
        /// </summary>
        public static Assembly DefaultCSharpAssembly
        {
            get
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SingleOrDefault(a => a.GetName().Name == "Assembly-CSharp");
            }
        }

        /// <summary>
        /// 获取默认的程序集中的类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetDefaultAssemblyType(string typeName)
        {
            return DefaultCSharpAssembly.GetType(typeName);
        }

        /// <summary>
        /// 获得程序集中的所有继承于某个类型的子类
        /// </summary>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static Type[] GetAllSubType(Type parentType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return assemblies.SelectMany(a => a.GetTypes())
                    .Where(x => x.IsSubclassOf(parentType)).ToArray()
                ;
        }
    }


    /// <summary>
    /// 反射扩展
    /// </summary>
    public static class ReflectionExtension
    {
        public static void Example()
        {
            // var selfType = ReflectionExtension.GetAssemblyCSharp().GetType("QFramework.ReflectionExtension");
            // selfType.LogInfo();
        }

        public static Assembly GetAssemblyCSharp()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                if (a.FullName.StartsWith("Assembly-CSharp,"))
                {
                    return a;
                }
            }

//            Log.E(">>>>>>>Error: Can\'t find Assembly-CSharp.dll");
            return null;
        }


        public static Assembly GetAssemblyCSharpEditor()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly a in assemblies)
            {
                if (a.FullName.StartsWith("Assembly-CSharp-Editor,"))
                {
                    return a;
                }
            }

//            Log.E(">>>>>>>Error: Can\'t find Assembly-CSharp-Editor.dll");
            return null;
        }

        /// <summary>
        /// 基于接口获得继承此接口的类
        /// </summary>
        /// <param name="p_InterfaceType"></param>
        /// <returns></returns>
        public static Type[] GetCSharpClassByInterface(Assembly assembly, Type p_InterfaceType)
        {
            if (!p_InterfaceType.IsInterface)
            {
                Debug.LogError($"{p_InterfaceType}Not Interface Type");
                return null;
            }

            Type[] result = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(p_InterfaceType))
                .Where(x => !x.IsAbstract)
                .ToArray();
            return result;
        }

        /// <summary>
        /// 基于接口获得继承此接口的类
        /// </summary>
        /// <param name="p_InterfaceType"></param>
        /// <returns></returns>
        public static Type[] GetCSharpClassByInterface(Type p_InterfaceType)
        {
            if (!p_InterfaceType.IsInterface)
            {
                Debug.LogError($"{p_InterfaceType}Not Interface Type");
                return null;
            }

            Type[] result = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(p_InterfaceType)))
                .Where(x => !x.IsAbstract)
                .ToArray();
            return result;
        }

        /// <summary>
        /// C#获取一个类在其所在的程序集中的所有子类
        /// </summary>
        /// <param name="parentType">给定的类型</param>
        /// <returns>所有子类类型</returns>
        public static List<Type> GetCSharpClassByParentClass(Assembly assembly, Type parentType)
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

        public static string GetTypeFullName(this Type returnType)
        {
            var name = returnType.Name;
            if (returnType.IsGenericType)
            {
                var genericType = returnType.GetGenericTypeDefinition();
                if (genericType == typeof(Task<>))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"Task<");
                    var arguments = returnType.GenericTypeArguments;
                    for (var i = 0; i < arguments.Length; i++)
                    {
                        if (i != 0)
                        {
                            sb.Append(',');
                        }

                        var argument = arguments[i];
                        sb.Append(argument.GetTypeFullName());
                    }

                    sb.Append(">");
                    name = sb.ToString();
                }
                else if (genericType.IsSubclassOf(typeof(Delegate)))
                {
                    // 处理泛型委托类型
                    StringBuilder sb = new StringBuilder();
                    var arguments = returnType.GetGenericArguments();
                    sb.Append($"{genericType.Name}<");
                    sb.Append(string.Join(", ", arguments.Select(a => a.GetTypeFullName())));
                    sb.Append(">");
                    name = sb.ToString();
                }
                else if (genericType.ImplementsInterface<IList>())
                {
                    // 处理泛型集合类型
                    StringBuilder sb = new StringBuilder();
                    var arguments = returnType.GetGenericArguments();
                    sb.Append($"{genericType.Name}<");
                    sb.Append(string.Join(", ", arguments.Select(a => a.GetTypeFullName())));
                    sb.Append(">");
                    name = sb.ToString();
                }
            }

            return name;
        }

        /// <summary>
        /// 通过反射方式调用函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object InvokeByReflect(this object obj, string methodName, params object[] args)
        {
            MethodInfo methodInfo = obj.GetType().GetMethod(methodName);
            return methodInfo == null ? null : methodInfo.Invoke(obj, args);
        }

        /// <summary>
        /// 通过反射方式获取域值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">域名</param>
        /// <returns></returns>
        public static object GetFieldByReflect(this object obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName);
            return fieldInfo == null ? null : fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// 通过反射方式获取属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyByReflect(this object obj, string propertyName, object[] index = null)
        {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, index);
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this PropertyInfo prop, Type attributeType, bool inherit)
        {
            return prop.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this FieldInfo field, Type attributeType, bool inherit)
        {
            return field.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this Type type, Type attributeType, bool inherit)
        {
            return type.GetCustomAttributes(attributeType, inherit).Length > 0;
        }

        /// <summary>
        /// 拥有特性
        /// </summary>
        /// <returns></returns>
        public static bool HasAttribute(this MethodInfo method, Type attributeType, bool inherit)
        {
            return method.GetCustomAttributes(attributeType, inherit).Length > 0;
        }


        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this MethodInfo method, bool inherit) where T : Attribute
        {
            T[] attrs = (T[])method.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this FieldInfo field, bool inherit) where T : Attribute
        {
            T[] attrs = (T[])field.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this PropertyInfo prop, bool inherit) where T : Attribute
        {
            T[] attrs = (T[])prop.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }

        /// <summary>
        /// 获取第一个特性
        /// </summary>
        public static T GetFirstAttribute<T>(this Type type, bool inherit) where T : Attribute
        {
            T[] attrs = (T[])type.GetCustomAttributes(typeof(T), inherit);
            if (attrs != null && attrs.Length > 0)
                return attrs[0];
            return null;
        }
    }

    /// <summary>
    /// 类型扩展
    /// </summary>
    public static class TypeEx
    {
        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object DefaultForType(this Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }

    public static class VectorExtension
    {
        public static void Example()
        {
        }

        public static float Max(this Vector3 value)
        {
            return Mathf.Max(value.x, value.y, value.z);
        }

        public static float Max(this Vector4 value)
        {
            return Mathf.Max(value.x, value.y, value.z, value.w);
        }

        public static Vector2 ToVector2(this Vector3 value, Axis newX, Axis newY)
        {
            return new Vector2(GetValueByAxis(value, newX), GetValueByAxis(value, newY));
        }

        public static Vector3 ToVector3(this Vector3 value, Axis newX, Axis newY, Axis newZ)
        {
            return new Vector3(GetValueByAxis(value, newX), GetValueByAxis(value, newY), GetValueByAxis(value, newZ));
        }

        public static Vector4 ToVector4(this Vector4 value, Axis newX, Axis newY, Axis newZ, Axis newW)
        {
            return new Vector4(GetValueByAxis(value, newX), GetValueByAxis(value, newY), GetValueByAxis(value, newZ),
                GetValueByAxis(value, newW));
        }

        public static float GetValueByAxis(this Vector2 value, Axis axis)
        {
            switch (axis)
            {
                case Axis.Zero:
                    return 0;
                case Axis.One:
                    return 1;
                case Axis.X:
                    return value.x;
                    break;
                case Axis.Y:
                    return value.y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static float GetValueByAxis(this Vector3 value, Axis axis)
        {
            switch (axis)
            {
                case Axis.Zero:
                    return 0;
                case Axis.One:
                    return 1;
                case Axis.X:
                    return value.x;
                    break;
                case Axis.Y:
                    return value.y;
                    break;
                case Axis.Z:
                    return value.z;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static float GetValueByAxis(this Vector4 value, Axis axis)
        {
            switch (axis)
            {
                case Axis.Zero:
                    return 0;
                case Axis.One:
                    return 1;
                case Axis.X:
                    return value.x;
                    break;
                case Axis.Y:
                    return value.y;
                    break;
                case Axis.Z:
                    return value.z;
                    break;
                case Axis.W:
                    return value.w;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        public static Vector4 ToVector4(this Vector3 value, float w)
        {
            return new Vector4(value.x, value.y, value.z, w);
        }

        public static Vector3 ToVector3(this Vector4 value, bool isDeviedByW = false)
        {
            if (isDeviedByW && Mathf.Abs(value.w) > 0.001f)
                return value / value.w;
            return value;
        }
    }

    public enum Axis
    {
        Zero,
        One,
        X,
        Y,
        Z,
        W
    }

    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtention
    {
        public static void Example()
        {
            string emptyStr = string.Empty;
            emptyStr.IsNotNullAndEmpty();
            emptyStr.IsNullOrEmpty();
            emptyStr = emptyStr.Append("appended").Append("1").ToString();
            emptyStr.IsNullOrEmpty();
        }

        /// <summary>
        /// 修正路径把 / 替换成 \
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathCorrect(this string path)
        {
            return path.Replace('/', '\\');
        }

        /// <summary>
        /// 用文件管理器打开 可以执行程序或打开文件夹
        /// </summary>
        /// <param name="path">正确的路径，如果错误，请用 PathCorrect 纠正一下</param>
        public static void ExplorerOpen(this string path)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "Explorer.exe";
            p.StartInfo.Arguments = path.PathCorrect();
            p.Start();
            p.WaitForExit();
            p.Close();
        }

        /// <summary>
        /// 移除特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Check Whether string is null or empty
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string selfStr)
        {
            return string.IsNullOrEmpty(selfStr);
        }

        /// <summary>
        /// Check Whether string is null or empty
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this string selfStr)
        {
            return !string.IsNullOrEmpty(selfStr);
        }

        /// <summary>
        /// Check Whether string trim is null or empty
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsTrimNotNullAndEmpty(this string selfStr)
        {
            return selfStr != null && !string.IsNullOrEmpty(selfStr.Trim());
        }

        public static bool IsTrimNullOrEmpty(this string selfStr)
        {
            return selfStr == null || string.IsNullOrEmpty(selfStr.Trim());
        }

        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly char[] mCachedSplitCharArray = { '.' };

        /// <summary>
        /// Split
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="splitSymbol"></param>
        /// <returns></returns>
        public static string[] Split(this string selfStr, char splitSymbol)
        {
            mCachedSplitCharArray[0] = splitSymbol;
            return selfStr.Split(mCachedSplitCharArray);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LowercaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnixLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        /// <summary>
        /// 转换成 CSV
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToCSV(this string[] values)
        {
            return string.Join(", ", values
                .Where(value => !string.IsNullOrEmpty(value))
                .Select(value => value.Trim())
                .ToArray()
            );
        }

        public static string[] ArrayFromCSV(this string values)
        {
            return values
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .ToArray();
        }

        public static string ToSpacedCamelCase(this string text)
        {
            StringBuilder sb = new StringBuilder(text.Length * 2);
            sb.Append(char.ToUpper(text[0]));
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                {
                    sb.Append(' ');
                }

                sb.Append(text[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 有点不安全,编译器不会帮你排查错误。
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FillFormat(this string selfStr, params object[] args)
        {
            return string.Format(selfStr, args);
        }

        /// <summary>
        /// 添加前缀
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toAppend"></param>
        /// <returns></returns>
        public static StringBuilder Append(this string selfStr, string toAppend)
        {
            return new StringBuilder(selfStr).Append(toAppend);
        }

        /// <summary>
        /// 添加后缀
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toPrefix"></param>
        /// <returns></returns>
        public static string AddPrefix(this string selfStr, string toPrefix)
        {
            return new StringBuilder(toPrefix).Append(selfStr).ToString();
        }

        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="toAppend"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StringBuilder AppendFormat(this string selfStr, string toAppend, params object[] args)
        {
            return new StringBuilder(selfStr).AppendFormat(toAppend, args);
        }

        /// <summary>
        /// 最后一个单词
        /// </summary>
        /// <param name="selfUrl"></param>
        /// <returns></returns>
        public static string LastWord(this string selfUrl)
        {
            return selfUrl.Split('/').Last();
        }

        /// <summary>
        /// 解析成数字类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaulValue"></param>
        /// <returns></returns>
        public static int ToInt(this string selfStr, int defaulValue = 0)
        {
            int retValue = defaulValue;
            return int.TryParse(selfStr, out retValue) ? retValue : defaulValue;
        }

        /// <summary>
        /// 解析到时间类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string selfStr, DateTime defaultValue = default(DateTime))
        {
            DateTime retValue = defaultValue;
            return DateTime.TryParse(selfStr, out retValue) ? retValue : defaultValue;
        }


        /// <summary>
        /// 解析 Float 类型
        /// </summary>
        /// <param name="selfStr"></param>
        /// <param name="defaulValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string selfStr, float defaulValue = 0)
        {
            float retValue = defaulValue;
            return float.TryParse(selfStr, out retValue) ? retValue : defaulValue;
        }

        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 是否存在空格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasSpace(this string input)
        {
            return input.Contains(" ");
        }

        /// <summary>
        /// 删除特定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemoveString(this string str, params string[] targets)
        {
            return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
        }

        public static void CopyToSystemBuffer(this string str)
        {
            GUIUtility.systemCopyBuffer = str;
            Debug.Log($"Copied to clipboard:{str}");
        }
    }

    public static class IntExtension
    {
        public static void Example()
        {
            int test = 0x0111;
            bool hasFlag = test.HasFlagByIndex(2); // 0x0011.HasFlag(0x0010)
            hasFlag = test.HasFlag(0x0100); // 等同上面的代码
        }

        /// <summary>
        /// 判断是否包含位Flag
        /// </summary>
        /// <param name="source"></param>
        /// <param name="flagValue">位Flag值</param>
        /// <returns></returns>
        public static bool HasFlag(this int source, int flagValue)
        {
            return (source & flagValue) != 0;
        }

        /// <summary>
        /// 基于位数,判断是否包含位Flag
        /// </summary>
        /// <param name="source"></param>
        /// <param name="flagIndex">从右向左数,从0开始</param>
        /// <returns></returns>
        public static bool HasFlagByIndex(this int source, int flagIndex)
        {
            return source.HasFlag(1 << flagIndex);
        }
    }

    /// <summary>
    /// Float拓展
    /// </summary>
    public static class FloatExtension
    {
        public static float MinMax(this float v, float min, float max)
        {
            return Mathf.Max(Mathf.Min(v, max), min);
        }

        /// <summary>
        /// 转整数 (自动进一位)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int CeilToInt(this float v)
        {
            return Mathf.CeilToInt(v);
        }
    }

    public static class ListExtension
    {
        public static string ToCSVString<T>(this List<T> list)
        {
            return string.Join(",", list.ToArray());
        }
    }


    public static class BehaviourExtension
    {
        public static void Example()
        {
            GameObject gameObject = new GameObject();
            MonoBehaviour component = gameObject.GetComponent<MonoBehaviour>();

            component.Enable(); // component.enabled = true
            component.Disable(); // component.enabled = false
        }

        public static T Enable<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = true;
            return selfBehaviour;
        }

        public static T Disable<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = false;
            return selfBehaviour;
        }
    }

    public static class CameraExtension
    {
        public static void Example()
        {
            Texture2D screenshotTexture2D = Camera.main.CaptureCamera(new Rect(0, 0, Screen.width, Screen.height));
        }

        public static Texture2D CaptureCamera(this Camera camera, Rect rect)
        {
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;

            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.Destroy(renderTexture);

            return screenShot;
        }
    }

    public static class ColorExtension
    {
        public static void Example()
        {
            Color color = "#C5563CFF".HtmlStringToColor();
        }

        /// <summary>
        /// #C5563CFF -> 197.0f / 255,86.0f / 255,60.0f / 255
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static Color HtmlStringToColor(this string htmlString)
        {
            Color retColor;
            bool parseSucceed = ColorUtility.TryParseHtmlString(htmlString, out retColor);
            return parseSucceed ? retColor : Color.black;
        }

        /// <summary>
        /// unity's color always new a color
        /// </summary>
        public static Color White = Color.white;
    }

    public static class MaterialExtension
    {
        public static void Example()
        {
            Material a = null, b = null;
            a.CopyFromMat(b);
        }

        public static void CopyFromMat(this Material source, Material target)
        {
            source.CopyPropertiesFromMaterial(target);
            source.color = target.color;
            source.mainTexture = target.mainTexture;
        }
    }

    public static class RendererExtension
    {
        public static void Example()
        {
            MeshRenderer m = new MeshRenderer();
            m.SwitchMaterials(Resources.Load<Material>("..."), true);
            m.SwitchBackMaterials();
        }

        private static Dictionary<Renderer, MaterialPack> materialOriginBackup = new();

        private class MaterialPack
        {
            public Material[] oldMats, newMats;

            public MaterialPack(Material[] oldMats, Material[] newMats)
            {
                this.oldMats = oldMats;
                this.newMats = newMats;
            }
        }

        public static void SwitchMaterials(this Renderer renderer, Material prefabMaterial, bool isRecord)
        {
            if (!materialOriginBackup.TryGetValue(renderer, out var pack))
            {
                var newMaterials = new Material[renderer.materials.Length];
                for (var i = 0; i < renderer.materials.Length; i++)
                {
                    var originMaterial = renderer.materials[i];
                    var newMaterial = new Material(prefabMaterial);
                    // newMaterial.SetFloat("_Metallic", originMaterial.GetFloat("_Metallic"));
                    // newMaterial.SetFloat("_Smoothness", originMaterial.GetFloat("_Smoothness"));
                    newMaterial.mainTexture = originMaterial.mainTexture;
                    newMaterial.color = originMaterial.color;
                    newMaterials[i] = newMaterial;
                }

                pack = new MaterialPack(renderer.materials, newMaterials);
                if (isRecord)
                {
                    materialOriginBackup.Add(renderer, pack);
                }
            }

            renderer.materials = pack.newMats;

            // foreach (var sourceMaterial in renderer.materials)
            // {
            //     if (materialOriginBackup.TryGetValue(sourceMaterial, out var pair))
            //     {
            //         // sourceMaterial.CopyPropertiesFromMaterial(pair.newMat);
            //         sourceMaterial.CopyFromMat(pair.newMat);
            //     }
            //     else
            //     {
            //         Material backup = new Material(sourceMaterial);
            //         var replaceMaterial = Object.Instantiate(prefabMaterial);
            //         replaceMaterial.mainTexture = backup.mainTexture;
            //         replaceMaterial.color = backup.color;
            //
            //         sourceMaterial.CopyFromMat(replaceMaterial);
            //
            //         if (isRecord)
            //         {
            //             pair = new MaterialPack(backup, replaceMaterial);
            //             materialOriginBackup[sourceMaterial] = pair;
            //         }
            //     }
            // }
        }

        public static void SwitchBackMaterials(this Renderer renderer)
        {
            if (materialOriginBackup.TryGetValue(renderer, out var pack))
                renderer.materials = pack.oldMats;
        }
    }

    public static class GraphicExtension
    {
        public static void Example()
        {
            GameObject gameObject = new GameObject();
            Image image = gameObject.AddComponent<Image>();
            RawImage rawImage = gameObject.AddComponent<RawImage>();

            // image.color = new Color(image.color.r,image.color.g,image.color.b,1.0f);
            image.ColorAlpha(1.0f);
            rawImage.ColorAlpha(1.0f);
        }

        public static T ColorAlpha<T>(this T selfGraphic, float alpha) where T : Graphic
        {
            Color color = selfGraphic.color;
            color.a = alpha;
            selfGraphic.color = color;
            return selfGraphic;
        }
    }

    public static class ImageExtension
    {
        public static void Example()
        {
            GameObject gameObject = new GameObject();
            Image image1 = gameObject.AddComponent<Image>();

            image1.FillAmount(0.0f); // image1.fillAmount = 0.0f;
        }

        public static Image FillAmount(this Image selfImage, float fillamount)
        {
            selfImage.fillAmount = fillamount;
            return selfImage;
        }
    }

    public static class LightmapExtension
    {
        public static void SetAmbientLightHTMLStringColor(string htmlStringColor)
        {
            RenderSettings.ambientLight = htmlStringColor.HtmlStringToColor();
        }
    }

    public static class ObjectExtension
    {
        public static void Example()
        {
            GameObject gameObject = new GameObject();

            gameObject.Instantiate()
                .Name("ExtensionExample")
                .DestroySelf();

            gameObject.Instantiate()
                .DestroySelfGracefully();

            gameObject.Instantiate()
                .DestroySelfAfterDelay(1.0f);

            gameObject.Instantiate()
                .DestroySelfAfterDelayGracefully(1.0f);

            gameObject
                .Name("TestObj")
                .Name("ExtensionExample")
                .DontDestroyOnLoad();
        }


        #region CEUO001 Instantiate

        public static T Instantiate<T>(this T selfObj) where T : Object
        {
            return Object.Instantiate(selfObj);
        }

        public static T Instantiate<T>(this T selfObj, Vector3 position, Quaternion rotation) where T : Object
        {
            return (T)Object.Instantiate((Object)selfObj, position, rotation);
        }

        public static T Instantiate<T>(
            this T selfObj,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
            where T : Object
        {
            return (T)Object.Instantiate((Object)selfObj, position, rotation, parent);
        }


        public static T InstantiateWithParent<T>(this T selfObj, Transform parent, bool worldPositionStays)
            where T : Object
        {
            return (T)Object.Instantiate((Object)selfObj, parent, worldPositionStays);
        }

        public static T InstantiateWithParent<T>(this T selfObj, Transform parent) where T : Object
        {
            return Object.Instantiate(selfObj, parent, false);
        }

        #endregion

        #region CEUO002 Name

        public static T Name<T>(this T selfObj, string name) where T : Object
        {
            selfObj.name = name;
            return selfObj;
        }

        #endregion

        #region CEUO003 Destroy Self

        public static void DestroySelf<T>(this T selfObj) where T : Object
        {
            Object.Destroy(selfObj);
        }

        public static T DestroySelfGracefully<T>(this T selfObj) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj);
            }

            return selfObj;
        }

        #endregion

        #region CEUO004 Destroy Self AfterDelay

        public static T DestroySelfAfterDelay<T>(this T selfObj, float afterDelay) where T : Object
        {
            Object.Destroy(selfObj, afterDelay);
            return selfObj;
        }

        public static T DestroySelfAfterDelayGracefully<T>(this T selfObj, float delay) where T : Object
        {
            if (selfObj)
            {
                Object.Destroy(selfObj, delay);
            }

            return selfObj;
        }

        #endregion

        #region CEUO005 Apply Self To

        public static T ApplySelfTo<T>(this T selfObj, System.Action<T> toFunction) where T : Object
        {
            toFunction.InvokeGracefully(selfObj);
            return selfObj;
        }

        #endregion

        #region CEUO006 DontDestroyOnLoad

        public static T DontDestroyOnLoad<T>(this T selfObj) where T : Object
        {
            Object.DontDestroyOnLoad(selfObj);
            return selfObj;
        }

        #endregion

        public static T As<T>(this object selfObj) where T : class
        {
            return selfObj as T;
        }
    }

    public static class RectTransformExtension
    {
        public static Vector2 GetPosInRootTrans(this RectTransform selfRectTransform, Transform rootTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(rootTrans, selfRectTransform).center;
        }

        public static RectTransform AnchorPosX(this RectTransform selfRectTrans, float anchorPosX)
        {
            Vector2 anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.x = anchorPosX;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform AnchorPosY(this RectTransform selfRectTrans, float anchorPosY)
        {
            Vector2 anchorPos = selfRectTrans.anchoredPosition;
            anchorPos.y = anchorPosY;
            selfRectTrans.anchoredPosition = anchorPos;
            return selfRectTrans;
        }

        public static RectTransform SetSizeWidth(this RectTransform selfRectTrans, float sizeWidth)
        {
            Vector2 sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.x = sizeWidth;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static RectTransform SetSizeHeight(this RectTransform selfRectTrans, float sizeHeight)
        {
            Vector2 sizeDelta = selfRectTrans.sizeDelta;
            sizeDelta.y = sizeHeight;
            selfRectTrans.sizeDelta = sizeDelta;
            return selfRectTrans;
        }

        public static Vector2 GetWorldSize(this RectTransform selfRectTrans)
        {
            return RectTransformUtility.CalculateRelativeRectTransformBounds(selfRectTrans).size;
        }

        /// <summary>
        /// 基于目标的世界位置,设置当前UI到对应位置上(角色头上血条这类使用)
        /// </summary>
        /// <param name="selfRectTrans"></param>
        /// <param name="worldPosition">3D物体的世界坐标</param>
        /// <param name="screenOffset"></param>
        /// <param name="uiCam"></param>
        /// <param name="parentRectTrans"></param>
        public static void SetUIPositionByTargetWorldTrans(this RectTransform selfRectTrans, Vector3 worldPosition,
            Vector2 screenOffset,
            Camera uiCam, RectTransform parentRectTrans)
        {
            // 将世界坐标转换到画布坐标
            Vector2 screenPosition = uiCam.WorldToScreenPoint(worldPosition);
            screenPosition += screenOffset;
            var screenPointToWorldPointInRectangle =
                RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRectTrans, screenPosition, uiCam,
                    out var worldPos);
            if (screenPointToWorldPointInRectangle)
                selfRectTrans.position = worldPos;
        }
    }

    public static class SelectableExtension
    {
        public static T EnableInteract<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.interactable = true;
            return selfSelectable;
        }

        public static T DisableInteract<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.interactable = false;
            return selfSelectable;
        }

        public static T CancalAllTransitions<T>(this T selfSelectable) where T : Selectable
        {
            selfSelectable.transition = Selectable.Transition.None;
            return selfSelectable;
        }
    }

    public static class ToggleExtension
    {
        public static void RegOnValueChangedEvent(this Toggle selfToggle, UnityAction<bool> onValueChangedEvent)
        {
            selfToggle.onValueChanged.AddListener(onValueChangedEvent);
        }
    }

    /// <summary>
    /// Transform's Extension
    /// </summary>
    public static class TransformExtension
    {
        public static void Example()
        {
            MonoBehaviour selfScript = new GameObject().AddComponent<MonoBehaviour>();
            Transform transform = selfScript.transform;

            transform
                // .Parent(null)
                .LocalIdentity()
                .LocalPositionIdentity()
                .LocalRotationIdentity()
                .LocalScaleIdentity()
                .LocalPosition(Vector3.zero)
                .LocalPosition(0, 0, 0)
                .LocalPosition(0, 0)
                .LocalPositionX(0)
                .LocalPositionY(0)
                .LocalPositionZ(0)
                .LocalRotation(Quaternion.identity)
                .LocalScale(Vector3.one)
                .LocalScaleX(1.0f)
                .LocalScaleY(1.0f)
                .Identity()
                .PositionIdentity()
                .RotationIdentity()
                .Position(Vector3.zero)
                .PositionX(0)
                .PositionY(0)
                .PositionZ(0)
                .Rotation(Quaternion.identity)
                .DestroyChildren()
                .AsLastSibling()
                .AsFirstSibling()
                .SiblingIndex(0);

            selfScript
                // .Parent(null)
                .LocalIdentity()
                .LocalPositionIdentity()
                .LocalRotationIdentity()
                .LocalScaleIdentity()
                .LocalPosition(Vector3.zero)
                .LocalPosition(0, 0, 0)
                .LocalPosition(0, 0)
                .LocalPositionX(0)
                .LocalPositionY(0)
                .LocalPositionZ(0)
                .LocalRotation(Quaternion.identity)
                .LocalScale(Vector3.one)
                .LocalScaleX(1.0f)
                .LocalScaleY(1.0f)
                .Identity()
                .PositionIdentity()
                .RotationIdentity()
                .Position(Vector3.zero)
                .PositionX(0)
                .PositionY(0)
                .PositionZ(0)
                .Rotation(Quaternion.identity)
                .DestroyChildren()
                .AsLastSibling()
                .AsFirstSibling()
                .SiblingIndex(0)
                .Parent(null)
                .AsRootTransform();
        }


        /// <summary>
        /// 缓存的一些变量,免得每次声明
        /// </summary>
        private static Vector3 mLocalPos;

        private static Vector3 mScale;
        private static Vector3 mPos;

        #region CETR001 Parent

        public static T Parent<T>(this T selfComponent, Component parentComponent) where T : Component
        {
            selfComponent.transform.SetParent(parentComponent == null ? null : parentComponent.transform);
            return selfComponent;
        }

        /// <summary>
        /// 设置成为顶端 Transform
        /// </summary>
        /// <returns>The root transform.</returns>
        /// <param name="selfComponent">Self component.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T AsRootTransform<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetParent(null);
            return selfComponent;
        }

        #endregion

        #region CETR002 LocalIdentity

        public static T LocalIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localPosition = Vector3.zero;
            selfComponent.transform.localRotation = Quaternion.identity;
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }

        #endregion

        #region CETR003 LocalPosition

        public static T LocalPosition<T>(this T selfComponent, Vector3 localPos) where T : Component
        {
            selfComponent.transform.localPosition = localPos;
            return selfComponent;
        }

        public static Vector3 GetLocalPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localPosition;
        }


        public static T LocalPosition<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.localPosition = new Vector3(x, y, z);
            return selfComponent;
        }

        public static T LocalPosition<T>(this T selfComponent, float x, float y) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.x = x;
            mLocalPos.y = y;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionX<T>(this T selfComponent, float x) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.x = x;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionY<T>(this T selfComponent, float y) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.y = y;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }

        public static T LocalPositionZ<T>(this T selfComponent, float z) where T : Component
        {
            mLocalPos = selfComponent.transform.localPosition;
            mLocalPos.z = z;
            selfComponent.transform.localPosition = mLocalPos;
            return selfComponent;
        }


        public static T LocalPositionIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localPosition = Vector3.zero;
            return selfComponent;
        }

        #endregion

        #region CETR004 LocalRotation

        public static Quaternion GetLocalRotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localRotation;
        }

        public static T LocalRotation<T>(this T selfComponent, Quaternion localRotation) where T : Component
        {
            selfComponent.transform.localRotation = localRotation;
            return selfComponent;
        }

        public static T LocalRotationIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localRotation = Quaternion.identity;
            return selfComponent;
        }

        #endregion

        #region CETR005 LocalScale

        public static T LocalScale<T>(this T selfComponent, Vector3 scale) where T : Component
        {
            selfComponent.transform.localScale = scale;
            return selfComponent;
        }

        public static Vector3 GetLocalScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.localScale;
        }

        public static T LocalScale<T>(this T selfComponent, float xyz) where T : Component
        {
            selfComponent.transform.localScale = Vector3.one * xyz;
            return selfComponent;
        }

        public static T LocalScale<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            mScale.y = y;
            mScale.z = z;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScale<T>(this T selfComponent, float x, float y) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            mScale.y = y;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleX<T>(this T selfComponent, float x) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.x = x;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleY<T>(this T selfComponent, float y) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.y = y;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleZ<T>(this T selfComponent, float z) where T : Component
        {
            mScale = selfComponent.transform.localScale;
            mScale.z = z;
            selfComponent.transform.localScale = mScale;
            return selfComponent;
        }

        public static T LocalScaleIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }

        #endregion

        #region CETR006 Identity

        public static T Identity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.position = Vector3.zero;
            selfComponent.transform.rotation = Quaternion.identity;
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }

        #endregion

        #region CETR007 Position

        public static T Position<T>(this T selfComponent, Vector3 position) where T : Component
        {
            selfComponent.transform.position = position;
            return selfComponent;
        }

        public static Vector3 GetPosition<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.position;
        }

        public static T Position<T>(this T selfComponent, float x, float y, float z) where T : Component
        {
            selfComponent.transform.position = new Vector3(x, y, z);
            return selfComponent;
        }

        public static T Position<T>(this T selfComponent, float x, float y) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = x;
            mPos.y = y;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.position = Vector3.zero;
            return selfComponent;
        }

        public static T PositionX<T>(this T selfComponent, float x) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = x;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionX<T>(this T selfComponent, Func<float, float> xSetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.x = xSetter(mPos.x);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionY<T>(this T selfComponent, float y) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.y = y;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionY<T>(this T selfComponent, Func<float, float> ySetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.y = ySetter(mPos.y);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionZ<T>(this T selfComponent, float z) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.z = z;
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        public static T PositionZ<T>(this T selfComponent, Func<float, float> zSetter) where T : Component
        {
            mPos = selfComponent.transform.position;
            mPos.z = zSetter(mPos.z);
            selfComponent.transform.position = mPos;
            return selfComponent;
        }

        #endregion

        #region CETR008 Rotation

        public static T RotationIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.rotation = Quaternion.identity;
            return selfComponent;
        }

        public static T Rotation<T>(this T selfComponent, Quaternion rotation) where T : Component
        {
            selfComponent.transform.rotation = rotation;
            return selfComponent;
        }

        public static Quaternion GetRotation<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.rotation;
        }

        #endregion

        #region CETR009 WorldScale/LossyScale/GlobalScale/Scale

        public static Vector3 GetGlobalScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetWorldScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        public static Vector3 GetLossyScale<T>(this T selfComponent) where T : Component
        {
            return selfComponent.transform.lossyScale;
        }

        #endregion

        #region CETR010 Destroy All Child

        [Obsolete("弃用啦 请使用 DestroyChildren")]
        public static T DestroyAllChild<T>(this T selfComponent) where T : Component
        {
            return selfComponent.DestroyChildren();
        }

        [Obsolete("弃用啦 请使用 DestroyChildren")]
        public static GameObject DestroyAllChild(this GameObject selfGameObj)
        {
            return selfGameObj.DestroyChildren();
        }

        public static T DestroyChildren<T>(this T selfComponent) where T : Component
        {
            int childCount = selfComponent.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                selfComponent.transform.GetChild(i).DestroyGameObjGracefully();
            }

            return selfComponent;
        }

        public static GameObject DestroyChildren(this GameObject selfGameObj)
        {
            int childCount = selfGameObj.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                selfGameObj.transform.GetChild(i).DestroyGameObjGracefully();
            }

            return selfGameObj;
        }

        #endregion

        #region CETR011 Sibling Index

        public static T AsLastSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsLastSibling();
            return selfComponent;
        }

        public static T AsFirstSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsFirstSibling();
            return selfComponent;
        }

        public static T SiblingIndex<T>(this T selfComponent, int index) where T : Component
        {
            selfComponent.transform.SetSiblingIndex(index);
            return selfComponent;
        }

        #endregion


        public static Transform FindByPath(this Transform selfTrans, string path)
        {
            return selfTrans.Find(path.Replace(".", "/"));
        }

        public static Transform SeekTrans(this Transform selfTransform, string uniqueName)
        {
            Transform childTrans = selfTransform.Find(uniqueName);

            if (null != childTrans)
                return childTrans;

            foreach (Transform trans in selfTransform)
            {
                childTrans = trans.SeekTrans(uniqueName);

                if (null != childTrans)
                    return childTrans;
            }

            return null;
        }

        public static T ShowChildTransByPath<T>(this T selfComponent, string tranformPath) where T : Component
        {
            selfComponent.transform.Find(tranformPath).gameObject.Show();
            return selfComponent;
        }

        public static T HideChildTransByPath<T>(this T selfComponent, string tranformPath) where T : Component
        {
            selfComponent.transform.Find(tranformPath).Hide();
            return selfComponent;
        }

        public static void CopyDataFromTrans(this Transform selfTrans, Transform fromTrans)
        {
            selfTrans.SetParent(fromTrans.parent);
            selfTrans.localPosition = fromTrans.localPosition;
            selfTrans.localRotation = fromTrans.localRotation;
            selfTrans.localScale = fromTrans.localScale;
        }

        /// <summary>
        /// 递归遍历子物体，并调用函数
        /// </summary>
        /// <param name="tfParent"></param>
        /// <param name="action"></param>
        public static void ActionRecursion(this Transform tfParent, Action<Transform> action)
        {
            action(tfParent);
            foreach (Transform tfChild in tfParent)
            {
                tfChild.ActionRecursion(action);
            }
        }

        /// <summary>
        /// 递归遍历查找指定的名字的子物体
        /// </summary>
        /// <param name="tfParent">当前Transform</param>
        /// <param name="name">目标名</param>
        /// <param name="stringComparison">字符串比较规则</param>
        /// <returns></returns>
        public static Transform FindChildRecursion(this Transform tfParent, string name,
            StringComparison stringComparison = StringComparison.Ordinal)
        {
            if (tfParent.name.Equals(name, stringComparison))
            {
                //Debug.Log("Hit " + tfParent.name);
                return tfParent;
            }

            foreach (Transform tfChild in tfParent)
            {
                Transform tfFinal = null;
                tfFinal = tfChild.FindChildRecursion(name, stringComparison);
                if (tfFinal)
                {
                    return tfFinal;
                }
            }

            return null;
        }

        /// <summary>
        /// 递归遍历查找相应条件的子物体
        /// </summary>
        /// <param name="tfParent">当前Transform</param>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public static Transform FindChildRecursion(this Transform tfParent, Func<Transform, bool> predicate)
        {
            if (predicate(tfParent))
            {
                Debug.Log("Hit " + tfParent.name);
                return tfParent;
            }

            foreach (Transform tfChild in tfParent)
            {
                Transform tfFinal = null;
                tfFinal = tfChild.FindChildRecursion(predicate);
                if (tfFinal)
                {
                    return tfFinal;
                }
            }

            return null;
        }

        public static string GetPath(this Transform transform)
        {
            StringBuilder sb = new System.Text.StringBuilder();
            Transform t = transform;
            while (true)
            {
                sb.Insert(0, t.name);
                t = t.parent;
                if (t)
                {
                    sb.Insert(0, "/");
                }
                else
                {
                    return sb.ToString();
                }
            }
        }

        /// <summary>
        /// 当前保持z轴朝向不变，Up方向朝向目标点
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="worldPosition"></param>
        /// <param name="wordForward"></param>
        public static void LookAtUp(this Transform transform, Vector3 worldPosition, Vector3 wordForward)
        {
            var direction = (worldPosition - transform.position).normalized;
            var newRight = Vector3.Cross(direction, wordForward);
            var newUp = Vector3.Cross(wordForward, newRight);
            transform.rotation = Quaternion.LookRotation(wordForward, newUp);
        }

        /// <summary>
        /// 当前保持z轴朝向不变，Right方向朝向目标点(未测试脚本)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="worldPosition"></param>
        /// <param name="wordForward"></param>
        public static void LookAtRight(this Transform transform, Vector3 worldPosition, Vector3 wordForward)
        {
            var direction = (worldPosition - transform.position).normalized;
            var newRight = Vector3.Cross(direction, wordForward);
            transform.rotation = Quaternion.LookRotation(wordForward, -newRight);
        }
    }

    public static class UnityActionExtension
    {
        public static void Example()
        {
            UnityAction action = () => { };
            UnityAction<int> actionWithInt = num => { };
            UnityAction<int, string> actionWithIntString = (num, str) => { };

            action.InvokeGracefully();
            actionWithInt.InvokeGracefully(1);
            actionWithIntString.InvokeGracefully(1, "str");
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully(this UnityAction selfAction)
        {
            if (null != selfAction)
            {
                selfAction();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool InvokeGracefully<T>(this UnityAction<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                selfAction(t);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Call action
        /// </summary>
        /// <param name="selfAction"></param>
        /// <returns> call succeed</returns>
        public static bool InvokeGracefully<T, K>(this UnityAction<T, K> selfAction, T t, K k)
        {
            if (null != selfAction)
            {
                selfAction(t, k);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得随机列表中元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }


        /// <summary>
        /// 根据权值来获取索引
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public static int GetRandomWithPower(this List<int> powers)
        {
            int sum = 0;
            foreach (int power in powers)
            {
                sum += power;
            }

            int randomNum = UnityEngine.Random.Range(0, sum);
            int currentSum = 0;
            for (int i = 0; i < powers.Count; i++)
            {
                int nextSum = currentSum + powers[i];
                if (randomNum >= currentSum && randomNum <= nextSum)
                {
                    return i;
                }

                currentSum = nextSum;
            }

            return -1;
        }

        /// <summary>
        /// 根据权值获取值，Key为值，Value为权值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="powersDict"></param>
        /// <returns></returns>
        public static T GetRandomWithPower<T>(this Dictionary<T, int> powersDict)
        {
            List<T> keys = new List<T>();
            List<int> values = new List<int>();

            foreach (T key in powersDict.Keys)
            {
                keys.Add(key);
                values.Add(powersDict[key]);
            }

            int finalKeyIndex = values.GetRandomWithPower();
            return keys[finalKeyIndex];
        }
    }
}