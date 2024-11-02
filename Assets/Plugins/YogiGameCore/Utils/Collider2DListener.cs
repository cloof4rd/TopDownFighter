using System;
using UnityEngine;

namespace YogiGameCore.Utils
{
    public class Collider2DListener : MonoBehaviour
    {
        private Collider2D _Collider;
        private Collider2D Collider => _Collider ? _Collider : _Collider = GetComponent<Collider2D>();
        public event Action<Collider2D> OnTriggerEnter, OnTriggerExit, OnTriggerStay;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTriggerEnter?.Invoke(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExit?.Invoke(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTriggerStay?.Invoke(collision);
        }
    }
}