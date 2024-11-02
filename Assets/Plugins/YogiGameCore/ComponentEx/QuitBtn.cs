using UnityEngine;
using YogiGameCore.Const;
using YogiGameCore.Utils;

namespace YogiGameCore.ComponentEx
{
    [RequireComponent(typeof(UberButton))]
    public class QuitBtn : MonoBehaviour
    {
        private UberButton _button;

        private void Awake()
        {
            _button = GetComponent<UberButton>();
            if (!_button.HasHold)
                _button.OnClicked += QuitGame;
            else
                _button.OnToggleValueChanged += (v) =>
                {
                    if (v) QuitGame();
                };
        }

        private void QuitGame()
        {
            EventCenter.Instance.EventTrigger(GlobalEventConst.GAME_OVER);
            
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
            Application.Quit();
        }
    }
}