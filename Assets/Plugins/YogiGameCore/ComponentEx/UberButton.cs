using System;
using System.Collections.Generic;
using UnityEngine;

namespace YogiGameCore.ComponentEx
{
    public class UberButton : MonoBehaviour
    {
        private bool m_IsEnable = true;
        public UIListener ImageBtnListener;
        [Header("切换图片")]
        public List<SwapImageSpritePack> SwapImagePacks = new List<SwapImageSpritePack>();
        [Header("切换UI颜色")]
        public List<ChangeGraphicColorPack> ChangeImagePacks = new List<ChangeGraphicColorPack>();
        private List<IChangePack> allPack;
        public event Action OnClicked;
        public event Action<bool> OnToggleValueChanged;
        public bool HasHold = false;
        public bool IsHold = false;

        public bool HasHoldKey = false;
        public string HoldGroupKey = "Default";

        private static Dictionary<string, List<UberButton>> HoldDic = new Dictionary<string, List<UberButton>>();
        
        private void OnValidate()
        {
            foreach (var changeGraphicColorPack in ChangeImagePacks)
            {
                if (changeGraphicColorPack.ChangeColors.Count == 0)
                {
                    changeGraphicColorPack.ChangeColors  = new List<Color>()
                    {
                        new Color(1,1,1,1),new Color(1,1,1,1),
                        new Color(0.7843137f,0.7843137f,0.7843137f,1),new Color(1,1,1,1),
                        new Color(0.7843137f,0.7843137f,0.7843137f,.5f),
                    };
                }
            }
            
            SwapImagePacks.ForEach(x =>
            {
                if (x.swapImage != null && x.swapSprites.Count == 0)
                {
                    x.swapSprites = new List<Sprite>();
                    for (int i = 0; i < (HasHold?6:5); i++)
                    {
                        x.swapSprites.Add(x.swapImage.sprite);
                    }
                }
            });

#if UNITY_EDITOR
            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
            {
                SetState(ChangeState.Normal);
            }
#endif
        }

        private void Awake()
        {
            // 记录同样的HoldKey 的hold按钮，方面后续实现 ToggleGroup
            if (HasHold && HasHoldKey)
            {
                if (!HoldDic.TryGetValue(HoldGroupKey,out List<UberButton> list))
                {
                    list = new List<UberButton>();
                    HoldDic.Add(HoldGroupKey,list);
                }
                list.Add(this);
                
            }
            
            allPack = new List<IChangePack>();
            allPack.AddRange(SwapImagePacks);
            allPack.AddRange(ChangeImagePacks);
            
            foreach (var pack in allPack)
            {
                pack.Init(ImageBtnListener);
            }

            ImageBtnListener.OnPointerLeftButtonClickedEvent += () =>
            {
                if (!m_IsEnable)
                    return;
                OnClicked?.Invoke();
            };
            if (HasHold)
            {
                ImageBtnListener.OnPointerLeftButtonClickedEvent += () =>
                {
                    if (!m_IsEnable)
                        return;
                    SetHold(!IsHold);
                };
            }
        }

        public void ClearAllListener()
        {
            OnClicked = null;
            OnToggleValueChanged = null;
        }

        public void TriggerClick()
        {
            OnClicked?.Invoke();
        }
        public void SetHold(bool value)
        {
            if (IsHold == value || !HasHold)
                return;
            IsHold = value;
            foreach (var pack in allPack)
                pack.SetHold(IsHold);
            // 取消其他同样HoldKey的按钮的Hold态
            if (HasHold && HasHoldKey && value)
            {
                if (HoldDic.TryGetValue(HoldGroupKey,out List<UberButton> list))
                {
                    list.ForEach(x =>
                    {
                        if (x!=null && x != this)
                        {
                            x.SetHold(false);
                        }
                    });
                }
            }
            
            OnToggleValueChanged?.Invoke(IsHold);
        }

        public void SetEnable(bool value)
        {
            if (m_IsEnable == value)
                return;
            m_IsEnable = value;
            foreach (var pack in allPack)
            {
                pack.SetEnable(m_IsEnable);
            }
        }
        
        public void SetState(ChangeState index)
        {
            foreach (var changePack in SwapImagePacks)
            {
                changePack.SetState(index);
            }

            foreach (var changePack in ChangeImagePacks)
            {
                changePack.SetState(index);
            }
        }
    }
}