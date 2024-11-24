using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput input;
    public int playerIndex;
    public Role[] roles;
    public Role currentControlRole;
    public bool isBlockInput = false;
    public Action onSubmit, onCancel;
    public static Action onRestartGame, onExitGame, onContinueGame, onPause;

    public static Action onPlayDieAnim, onPlayTakeDamageAnim, onSwitchNextRole, onSwitchPrevRole;

    public void SwitchToCommonInput()
    {
        input.SwitchCurrentActionMap("Gameplay");
    }
    public void SwitchToUIInput()
    {
        input.SwitchCurrentActionMap("UI");
    }
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;
        if (roles.Length > playerIndex)
            SwitchRole(roles[playerIndex]);
    }
    public void SwitchRole(Role role)
    {
        currentControlRole = role;
        role.playerIndex = playerIndex;
    }
    public void OnMove(InputValue v)
    {
        if (isBlockInput)
            return;
        var data = currentControlRole.inputData;
        data.moveDir = v.Get<Vector2>();
        data.isMoveing = data.moveDir.sqrMagnitude > 0.1f;
    }
    public async void OnAttack1()
    {
        if (isBlockInput)
            return;
        currentControlRole.inputData.isAttack1 = true;
        await Task.Yield();
        currentControlRole.inputData.isAttack1 = false;
    }
    public async void OnAttack2()
    {
        if (isBlockInput)
            return;
        currentControlRole.inputData.isAttack2 = true;
        await Task.Yield();
        currentControlRole.inputData.isAttack2 = false;
    }
    public async void OnAttack3()
    {
        if (isBlockInput)
            return;
        currentControlRole.inputData.isAttack3 = true;
        await Task.Yield();
        currentControlRole.inputData.isAttack3 = false;
    }
    public async void OnAttack4()
    {
        if (isBlockInput)
            return;
        currentControlRole.inputData.isAttack4 = true;
        await Task.Yield();
        currentControlRole.inputData.isAttack4 = false;
    }
    public async void OnBlockSkill()
    {
        if (isBlockInput)
            return;
        currentControlRole.inputData.isBlockSkill = true;
        await Task.Yield();
        currentControlRole.inputData.isBlockSkill = false;
    }

    public void OnLockSkill(InputValue v)
    {
        print(v.Get<float>());
        isBlockInput = v.Get<float>() > 0;
    }
    public void OnPause()
    {
        onPause?.Invoke();
    }
    private void OnSubmit()
    {
        onSubmit?.Invoke();
        print("OnSubmit");
    }
    private void OnCancel()
    {
        onCancel?.Invoke();
        print("OnCancel");
    }

    private void OnRestartGame()
    {
        onRestartGame?.Invoke();
    }
    private void OnExitGame()
    {
        onExitGame?.Invoke();
    }
    private void OnContinue()
    {
        onContinueGame?.Invoke();
    }



    private void OnSwitchPrevRole()
    {
        onSwitchPrevRole?.Invoke();
    }
    private void OnSwitchNextRole()
    {
        onSwitchNextRole?.Invoke();
    }
    private void OnPlayDieAnim()
    {
        onPlayDieAnim?.Invoke();
    }
    private void OnPlayTakeDamageAnim()
    {
        onPlayTakeDamageAnim?.Invoke();
    }

}
