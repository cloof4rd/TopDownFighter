/*
 * Unity Menu:  Editor/Preferences
 * Set:   Preferences.AutoRefresh = false
 */

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace YogiGameCore.Editor
{
    public static class ShortCuts
    {
        //[MenuItem("Tools/Clear Console %#x")]
        [MenuItem("Tools/Unity Console/Clear Console %q")]
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ActiveEditorTracker));
            Type type = assembly.GetType("UnityEditorInternal.LogEntries");
            if (type == null)
            {
                type = assembly.GetType("UnityEditor.LogEntries");
            }

            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }

        [MenuItem("Tools/Unity Console/Clear Console And Refresh %r")]
        public static void ClearConsoleAndRefresh()
        {
            ClearConsole();
            AssetDatabase.Refresh();
            
        }

        [MenuItem("Tools/Prefab/Override Select Scene Prefab %#s")]
        public static void SaveToAsset()
        {
            Save();
        }

        public static void Save()
        {
            if (Selection.activeGameObject == null)
                return;
            var go = Selection.activeGameObject;
            var sorceGO = PrefabUtility.GetCorrespondingObjectFromSource(go);

            var path = AssetDatabase.GetAssetPath(sorceGO).ToLower();
            PrefabUtility.SaveAsPrefabAsset(go, path, out var isSuccess);
            Debug.Log(isSuccess ? $"Save {go.gameObject} success {path}" : $"Save {go.gameObject} failed {path}", go);
        }
    }
}