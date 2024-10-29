using UnityEngine;
using YogiGameCore.Utils;
using YogiGameCore.Utils.MonoExtent;

namespace YogiGameCore.ComponentEx
{
    public class GUILabelDisplay : MonoBehaviour
    {
        public string DisplayText;

    
        [SerializeField]
        private int m_InitFontSize = 22;
        [SerializeField]
        private Color m_InitFontColor = Color.red;
        private Camera mainCam;
        private GUIStyle fontStyle;
        private Vector2 fontArea;

        public bool IsHideByDistance = true;
        private float HideDistance = 50;
        
        private void Awake()
        {
            mainCam = Camera.main;
            fontStyle = new GUIStyle
            {
                fontSize = m_InitFontSize,
                normal =
                {
                    textColor = m_InitFontColor,
                    background = Texture2D.grayTexture,
                },
                alignment = TextAnchor.MiddleCenter,
            };
            
        }
        [Button]
        public void SetFontSize(int size)
        {
            fontStyle.fontSize = size;
        }
        [Button]
        public void SetFontColor(Color color)
        {
            fontStyle.normal.textColor = color;
        }

        private void OnGUI()
        {
            if (DisplayText.IsNullOrEmpty())
                return;
            var worldToScreenPoint = mainCam.WorldToScreenPoint(this.transform.position);
            if (worldToScreenPoint.z < 0)
                return;
            if (IsHideByDistance && HideDistance < Vector3.Distance(mainCam.transform.position, this.transform.position))
                return;
            
            var pos = new Vector2(worldToScreenPoint.x,Screen.height-worldToScreenPoint.y);
            GUI.Label(new Rect(pos, Vector2.zero),DisplayText, fontStyle);
            // GUI.bac
        }
    }
}
