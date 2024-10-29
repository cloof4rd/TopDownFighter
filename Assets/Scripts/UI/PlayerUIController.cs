using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    public int playerIndex;
    public Action<Vector2> onPlayerNavigate;
    private void Awake()
    {
        playerIndex = GetComponent<PlayerInput>().playerIndex;
    }
    public void OnNavigate(InputValue v)
    {
        onPlayerNavigate?.Invoke(v.Get<Vector2>());
    }
}
