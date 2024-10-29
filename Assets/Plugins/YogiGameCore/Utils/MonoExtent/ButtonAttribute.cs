using System;
using UnityEngine;

namespace YogiGameCore.Utils.MonoExtent
{
    /// <summary>
    /// 编辑器按钮特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string Text;
        public Color DisplayColor = Color.white;

        public ButtonAttribute()
        {
        }
        public ButtonAttribute(string text = "")
        {
            Text = text;
        }
        public ButtonAttribute(Color color,string text = "")
        {
            Text = text;
            DisplayColor = color;
        }
    }
}