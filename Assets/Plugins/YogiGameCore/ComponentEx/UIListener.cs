using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YogiGameCore.ComponentEx
{
    /// <summary>
    /// UI交互监听
    /// </summary>
    public class UIListener : MonoBehaviour, IPointerEnterHandler,
        IPointerExitHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerMoveHandler, IPointerClickHandler
    {
        public event Action
            OnPointerEnterEvent,
            OnPointerExitEvent,
            OnPointerLeftButtonDownEvent,
            OnPointerButtonDownEvent,
            OnPointerUpEvent,
            OnPointerMoveEvent,
            OnPointerLeftButtonClickedEvent,
            OnPointerButtonClickedEvent;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitEvent?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnPointerLeftButtonDownEvent?.Invoke();
            }
            
            OnPointerButtonDownEvent?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEvent?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            OnPointerMoveEvent?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnPointerLeftButtonClickedEvent?.Invoke();
            }
            OnPointerButtonClickedEvent?.Invoke();
        }
    }
}