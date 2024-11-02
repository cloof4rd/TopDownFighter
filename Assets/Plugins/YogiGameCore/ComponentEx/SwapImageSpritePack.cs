using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YogiGameCore.Utils.MonoExtent;

namespace YogiGameCore.ComponentEx
{
    [System.Serializable]
    public class SwapImageSpritePack : IChangePack
    {
        private bool m_IsEnable = true;
        [Header("目标图片")] public Image swapImage;

        [Header("0: 常态 1:移入 2:按下 3:选择 4:禁用 5:Hold")]
        public List<Sprite> swapSprites;

        bool HasHold => swapSprites.Count > 5 && isHold;
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
            if (swapImage != null)
            {
                SetState(m_IsEnable ? ChangeState.Normal : ChangeState.Disabled);
            }
        }

        public void Init(UIListener ImageBtnListener)
        {
            if (swapImage == null)
                return;
            SetState(ChangeState.Normal);
            ImageBtnListener.OnPointerEnterEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                SetState(!HasHold ? ChangeState.Highlight : ChangeState.Hold);
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
            if (swapImage != null)
            {
                var i = (int)index;
                if (swapSprites.Count > i)
                {
                    swapImage.sprite = swapSprites[i];
                    swapImage.SetNativeSize();
                }

            }
        }
    }
}