using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using YogiGameCore.Utils;

namespace YogiGameCore.Const
{
    /// <summary>
    /// Resources内的资源目录 可以配置
    /// </summary>
    [CreateAssetMenu(fileName = "GamePathConfig", menuName = "GameConfig", order = 0)]
    public class GameResourceConfig : ScriptableObject
    {
        public string ScriptNamespace = String.Empty;

        public string ScriptUINamespace => ScriptNamespace + ".UI";

        //Assets下的路径
        public string UIScriptsPath => $"Scripts/{ScriptNamespace}/UI/Panel";

        // Resource下的路径
        public string UIPanelPath = "Prefabs/UI/Panel";
        public string SingleUIPath = "Prefabs/UI/SingleUI";
        public string AbsoluteUIScriptsPath => $"{Application.dataPath}/{UIScriptsPath}";
        public string RelativeUIScriptsPath => $"Assets/{UIScriptsPath}";
        public string AbsoluteUIPanelPath => $"{Application.dataPath}/{UIPanelPath}";
        public string RelativeUIPanelPath => $"Assets/Resources/{UIPanelPath}";

        public string GameCorePath = "GameCore";


        //GAS Config
        public string AllTags;
        public string AbsoluteGASConfigExcelPath = $"{Application.dataPath}/Resources/Configs/Excels";
        public string GASConfigPath = "Configs/GASConfig";
        public string AbsoluteGASConfigPath => $"{Application.dataPath}/Resources/Configs/GASConfig";
        public string GASModifyConfigPath => $"{GASConfigPath}/Modify";
        public string GASCueConfigPath => $"{GASConfigPath}/Cue";
        public string GASEffectConfigPath => $"{GASConfigPath}/Effect";
        public string GASAbilityConfigPath => $"{GASConfigPath}/Ability";

        // Ultimate Graph Config
        public string AbsoluteGraphConfigPath => $"{Application.dataPath}/Resources/Configs/GraphConfig";
        public string GraphConfigPath = "Configs/GraphConfig";

        private void OnValidate()
        {
            if (ScriptNamespace == String.Empty)
            {
                // ScriptNamespace=
                var strings = Application.dataPath.Split('/');
                var nameSpace = strings[strings.Length - 2];
                nameSpace = nameSpace.RemoveSpecialCharacters();
                ScriptNamespace = nameSpace;
            }

            var exists = Directory.Exists(AbsoluteGASConfigPath);
            if (!exists)
            {
#if UNITY_EDITOR
                if (!EditorUtility.DisplayDialog("GASConfigPath Not Exists",
                        $"GASConfigPath:{AbsoluteGASConfigPath} Not Exists, Create GASConfigPath?", "OK", "NO")) return;
                Directory.CreateDirectory(AbsoluteGASConfigPath);
                AssetDatabase.Refresh();
#endif
            }
        }

        
    }
}