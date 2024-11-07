using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerUIController : MonoBehaviour
{
    public int playerIndex;
    public Action<Vector2> onPlayerNavigate;

    public InputSystemUIInputModule[] modules;

    private void Awake()
    {
        playerIndex = GetComponent<PlayerInput>().playerIndex;
        if (modules.Length > 1)
        {
            this.GetComponent<PlayerInput>().uiInputModule = modules[this.playerIndex];
        }

    }

    public void OnNavigate(InputValue v)
    {
        onPlayerNavigate?.Invoke(v.Get<Vector2>());
    }
}
