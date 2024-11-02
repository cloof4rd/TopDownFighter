using UnityEditor;
using UnityEngine;
using YogiGameCore.Utils;

namespace YogiGameCore.Editor
{
    public static class OpenFolder
    {
        [MenuItem("Tools/Path/Open Persistent Data Path",priority = 1)]
        public static void OpenPersistentPath()
        {
            Application.persistentDataPath.ExplorerOpen();
        }
        
        [MenuItem("Tools/Path/Open Streaming Assets Path",priority = 2)]
        public static void OpenStreamingAssetsPath()
        {
            Application.streamingAssetsPath.ExplorerOpen();
        }
        
        [MenuItem("Tools/Path/Open Data Path",priority = 3)]
        public static void OpenDataPath()
        {
            Application.dataPath.ExplorerOpen();
        }
        
        [MenuItem("Tools/Path/Open Temporary Cache Path")]
        public static void OpenTemporaryCachePath()
        {
            Application.temporaryCachePath.ExplorerOpen();
        }
        
        [MenuItem("Tools/Path/Open Console Log Path")]
        public static void OpenConsoleLogPath()
        {
            Application.consoleLogPath.ExplorerOpen();
        }
    }
}