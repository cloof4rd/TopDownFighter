using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YogiGameCore.Utils.MonoExtent;

namespace YogiGameCore.ComponentEx
{
    [System.Serializable]
    public class ChangeGraphicColorPack : IChangePack
    {
        private bool m_IsEnable = true;
        [Header("目标UI集合")] public List<Graphic> ColorChangeGraphicArr = new List<Graphic>();
        [Header("0: 常态 1:移入 2:按下 3:选择 4:禁用 5:Hold")] public List<Color> ChangeColors;
        bool HasHold => ChangeColors.Count > 5 && isHold;
        private bool isHold = false;

        public void SetHold(bool value)
        {
            isHold = value;
            SetState(!HasHold ? ChangeState.Normal : ChangeState.Hold);
        }
        
        [Button]
        public void SetEnable(bool isEnable)
        {
            m_IsEnable = isEnable;
            ColorChangeGraphicArr.ForEach(x => x.color = m_IsEnable ? ChangeColors[0] : ChangeColors[4]);
        }

        public void Init(UIListener ImageBtnListener)
        {
            SetState(ChangeState.Normal);
            ImageBtnListener.OnPointerEnterEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                SetState(ChangeState.Highlight);
            };
            ImageBtnListener.OnPointerExitEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                SetState(!HasHold ? ChangeState.Normal : ChangeState.Hold);
            };
            ImageBtnListener.OnPointerLeftButtonDownEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                SetState(ChangeState.Pressed);
            };
            ImageBtnListener.OnPointerUpEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                SetState(!HasHold ? ChangeState.Selected : ChangeState.Hold);
            };
        }

        public void SetState(ChangeState index)
        {
            foreach (var ColorChangeImage in ColorChangeGraphicArr)
                if(ColorChangeImage!=null)
                    ColorChangeImage.color = ChangeColors[(int)index];
        }
    }
}