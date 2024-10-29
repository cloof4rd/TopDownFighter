using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using YogiGameCore.Log;
using Object = UnityEngine.Object;

namespace YogiGameCore.Editor
{
    public static class ScriptAndPrefabCreationTool
    {
        private const string NEW_SCRIPT_KEY = "NEW_SCRIPT_KEY";
        private const string NEW_PREFAB_PATH_KEY = "NEW_PREFAB_PATH_KEY";
        private const string NEW_ASSEMBLY_NAME_KEY = "NEW_ASSEMBLY_NAME_KEY";

        /// <summary>
        /// Test method to create a new script and prefab.
        /// </summary>
        public static void Test()
        {
            var assemblyName = "Assembly-CSharp";
            var namespaceName = "GameLogic";
            var scriptName = "MyScript";
            var prefabName = "MyPrefab";
            var scriptPath = $"Assets/Resources/{scriptName}.cs";
            var prefabPath = $"Assets/Resources/{prefabName}.prefab";
            var scriptContent = $@"
using UnityEngine;

namespace {namespaceName}
{{
    public class {scriptName} : MonoBehaviour
    {{
        void Start()
        {{
            Debug.Log(""Hello, Haha!"");
        }}
    }}
}}";
            Action<GameObject> gameObjectHandle = (go) => { go.AddComponent<BoxCollider>(); };
            CreateNewScriptAndPrefab(assemblyName, namespaceName, scriptName, scriptContent, scriptPath, prefabName,
                prefabPath, gameObjectHandle);
        }

        /// <summary>
        /// Creates a new script and prefab with the given parameters.
        /// </summary>
        public static void CreateNewScriptAndPrefab(string assemblyName, string namespaceName, string scriptName,
            string scriptContent, string scriptPath, string prefabName, string prefabPath,
            Action<GameObject> prefabHandle = null)
        {
            EditorPrefs.SetString(NEW_ASSEMBLY_NAME_KEY, assemblyName);
            CreateScript(namespaceName, scriptName, scriptContent, scriptPath);
            CreatePrefab(prefabName, prefabPath, prefabHandle);
            CompileAndSave();
        }

        private static void CreateScript(string namespaceName, string scriptName, string scriptContent,
            string scriptPath)
        {
            EditorPrefs.SetString(NEW_SCRIPT_KEY, namespaceName + "." + scriptName);
            File.WriteAllText(scriptPath, scriptContent);
        }

        private static void CreatePrefab(string prefabName, string prefabPath, Action<GameObject> prefabHandle)
        {
            GameObject newObject = new GameObject(prefabName);
            prefabHandle?.Invoke(newObject);
            EditorPrefs.SetString(NEW_PREFAB_PATH_KEY, prefabPath);
            PrefabUtility.SaveAsPrefabAssetAndConnect(newObject, prefabPath, InteractionMode.UserAction);
            Object.DestroyImmediate(newObject);
        }

        private static void CompileAndSave()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [InitializeOnLoadMethod]
        private static void WaitForCompileAndAddComponent()
        {
            if (EditorApplication.isCompiling)
            {
                return;
            }

            if (!EditorPrefs.HasKey(NEW_SCRIPT_KEY))
            {
                return;
            }

            var fullScriptName = EditorPrefs.GetString(NEW_SCRIPT_KEY);
            var assemblyName = EditorPrefs.GetString(NEW_ASSEMBLY_NAME_KEY);
            var componentType = Type.GetType(fullScriptName + $",{assemblyName}");

            if (componentType == null)
            {
                //LogCore.Log("componentType == null");
                return;
            }

            var prefabPath = EditorPrefs.GetString(NEW_PREFAB_PATH_KEY);
            AddComponentToPrefab(prefabPath, componentType);
        }

        private static void AddComponentToPrefab(string prefabPath, Type componentType)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.Log("prefab == null");
                return;
            }

            var instantiatedPrefab = GameObject.Instantiate(prefab);
            instantiatedPrefab.AddComponent(componentType);
            SavePrefab(instantiatedPrefab, prefabPath);
        }

        private static void SavePrefab(GameObject gameObject, string prefabPath)
        {
            PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
            Object.DestroyImmediate(gameObject);
            AssetDatabase.SaveAssets();
            Debug.Log("Done");
            EditorPrefs.DeleteKey(NEW_SCRIPT_KEY);
            EditorPrefs.DeleteKey(NEW_PREFAB_PATH_KEY);
            EditorPrefs.DeleteKey(NEW_ASSEMBLY_NAME_KEY);
        }
    }
}