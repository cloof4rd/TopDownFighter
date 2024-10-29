using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace YogiGameCore.Editor.CleanEmptyDir
{
    [InitializeOnLoad]
    public class Core : UnityEditor.AssetModificationProcessor
    {
        const string CLEAN_ON_SAVE_KEY = "k1";
        const string CLEAN_MAX_COUNT = "CLEAN_MAX_COUNT";
        static bool cleanOnSave;

        public static event Action OnAutoClean;

        public static string[] OnWillSaveAssets(string[] paths)
        {
            if (CleanOnSave)
            {
                int r = EditorPrefs.GetInt("RefreshCount");
                if (r > 0)
                {
                    r--;
                    EditorPrefs.SetInt("RefreshCount", r);
                    return paths;
                }
                else
                {
                    EditorPrefs.SetInt("RefreshCount", EditorPrefs.GetInt(CLEAN_MAX_COUNT));
                }


                List<DirectoryInfo> emptyDirs;
                FillEmptyDirList(out emptyDirs);
                if (emptyDirs != null && emptyDirs.Count > 0)
                {
                    DeleteAllEmptyDirAndMeta(ref emptyDirs);
                    Debug.Log("自动删除空文件夹,想要关闭在   [自定义工具/清除空文件夹]  窗口");

                    if (OnAutoClean != null)
                        OnAutoClean();
                }
            }

            return paths;
        }


        public static bool CleanOnSave
        {
            get
            {
                return EditorPrefs.GetBool(CLEAN_ON_SAVE_KEY, false);
            }
            set
            {
                EditorPrefs.SetBool(CLEAN_ON_SAVE_KEY, value);
            }
        }


        public static void DeleteAllEmptyDirAndMeta(ref List<DirectoryInfo> emptyDirs)
        {
            foreach (DirectoryInfo dirInfo in emptyDirs)
            {
                AssetDatabase.MoveAssetToTrash(GetRelativePathFromCd(dirInfo.FullName));
                Debug.Log($"[清除] 空目录:  [{dirInfo.Name}]    ");
            }

            emptyDirs = null;
        }

        public static void FillEmptyDirList(out List<DirectoryInfo> emptyDirs)
        {
            List<DirectoryInfo> newEmptyDirs = new List<DirectoryInfo>();
            emptyDirs = newEmptyDirs;

            DirectoryInfo assetDir = new DirectoryInfo(Application.dataPath);

            WalkDirectoryTree(assetDir, (dirInfo, areSubDirsEmpty) =>
            {
                bool isDirEmpty = areSubDirsEmpty && DirHasNoFile(dirInfo);
                if (isDirEmpty)
                    newEmptyDirs.Add(dirInfo);
                return isDirEmpty;
            });
        }
        

        // return: Is this directory empty?
        delegate bool IsEmptyDirectory(DirectoryInfo dirInfo, bool areSubDirsEmpty);


            
        // return: Is this directory empty?
        static bool WalkDirectoryTree(DirectoryInfo root, IsEmptyDirectory pred)
        {
            DirectoryInfo[] subDirs = root.GetDirectories();

            // ignore .DirectoryName directory
            if (IsIgnoreDirectory(root))
            {
                return false;
            }
            
            bool areSubDirsEmpty = true;
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                if (false == WalkDirectoryTree(dirInfo, pred))
                    areSubDirsEmpty = false;
            }

            bool isRootEmpty = pred(root, areSubDirsEmpty);
            return isRootEmpty;
        }

        static bool DirHasNoFile(DirectoryInfo dirInfo)
        {
            FileInfo[] files = null;

            try
            {
                files = dirInfo.GetFiles("*.*");
                files = files.Where(x => !IsMetaFile(x.Name) && !IsSystemFile(x.Name)).ToArray();
            }
            catch (Exception)
            {
            }

            return files == null || files.Length == 0;
        }
        // return: Is this directory not have head '.'
        static bool IsIgnoreDirectory(DirectoryInfo dirInfo)
        {
            if (dirInfo.Name[0] == '.')
            {
                return true;
            }

            return false;
        } 

        static string GetRelativePathFromCd(string filespec)
        {
            return GetRelativePath(filespec, Directory.GetCurrentDirectory());
        }
       
        public static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        static string GetMetaFilePath(string dirPath)
        {
            // TODO: remove ending slash
            return dirPath + ".meta";
        }

        static bool IsMetaFile(string path)
        {
            return path.EndsWith(".meta");
        }

        static bool IsSystemFile(string path)
        {
            return path.StartsWith(".");
        }
    }
}

