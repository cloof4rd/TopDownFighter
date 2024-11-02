using System;
using Aya.DataBinding;
using TMPro;
using UnityEngine;
using YogiGameCore.Utils;

namespace YogiGameCore.ComponentEx
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SliderBindToText : MonoBehaviour
    {
        public string content = "Default";
        public string key;
        public string format = "{0}%";
        private TextMeshProUGUI textMeshProUGUI;
        private void Awake()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            Action<float> v = OnValueChange;
            UBind.BindTarget(content, key, v).UpdateTarget();
        }

        private void OnValueChange(float v)
        {
            v *= 100;
            var intValue = (int)v;
            textMeshProUGUI.text = $"{format}".FillFormat(intValue);
        }
    }
}