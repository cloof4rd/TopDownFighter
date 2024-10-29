using System;
using UnityEngine;
using UnityEngine.InputSystem;
using YogiGameCore.Utils;

[Obsolete]
public class InputManager : MonoSingleton<InputManager>
{
    public CommonInputActions inputActions;
    private CommonInputActions.GameplayActions gameplay;

    public Vector2 moveDir;
    public bool isMoveing;
    public bool isAttack1;

    private void Awake()
    {
        inputActions = new CommonInputActions();
        gameplay = inputActions.Gameplay;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    private void Update()
    {
        moveDir = gameplay.Move.ReadValue<Vector2>();
        isMoveing = moveDir.sqrMagnitude > 0.1f;
        isAttack1 = gameplay.Attack1.IsPressed();
    }
}
