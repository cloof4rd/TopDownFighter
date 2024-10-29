using System;
using UnityEngine;

namespace YogiGameCore.Utils
{
    /// <summary>
    /// 鼠标事件监听
    /// </summary>
    public class MouseListener : MonoBehaviour
    {
        public event Action OnClicked,OnMouseDownEvent,
            OnMouseUpEvent,OnMouseEnterEvent,OnMouseExitEvent,
            OnMouseDragEvent, OnMouseOverEvent;

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
        }

        private void OnMouseUp()
        {
            OnMouseUpEvent?.Invoke();
        }

        private void OnMouseEnter()
        {
            print("MosueEnter");
            OnMouseEnterEvent?.Invoke();
        }

        private void OnMouseExit()
        {
            OnMouseExitEvent?.Invoke();
        }

        private void OnMouseUpAsButton()
        {
            OnClicked?.Invoke();
        }

        private void OnMouseDrag()
        {
            OnMouseDragEvent?.Invoke();
        }

        private void OnMouseOver()
        {
            OnMouseOverEvent?.Invoke();
        }
    }
}