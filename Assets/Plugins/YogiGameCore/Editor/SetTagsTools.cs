using System;
using UnityEditor;
using UnityEngine.UIElements;
using YogiGameCore.Utils;

namespace YogiGameCore.Editor
{
    public static class SetTagsTools
    {
        
    }
    
    public class GameObjectSetWindow : EditorWindow
    {
        [MenuItem("Tools/Tag")]
        public static void SetTags()
        {
            EditorWindow.GetWindow<GameObjectSetWindow>();
            // var selectTarget = Selection.activeGameObject;
            // selectTarget.TagAllChild("Untagged");
        }

        private void OnEnable()
        {
            var tagInput = new TextField("修改选择物体的所有子物体的Tag");
            this.rootVisualElement.Add(tagInput);
            var setUpTagBtn = new Button();
            setUpTagBtn.text = "修改选中的物体和他所有子物体Tag";
            this.rootVisualElement.Add(setUpTagBtn);
            
            setUpTagBtn.clicked += () =>
            {
                var gameObjects = Selection.gameObjects;
                // var selectTarget = Selection.activeGameObject;
                gameObjects.ForEach(x => x.TagAllChild(tagInput.text));
            };
        }
    }
}