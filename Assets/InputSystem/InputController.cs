using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput input;
    public int playerIndex;
    public Role[] roles;
    public Role currentControlRole;
    public bool isBlockInput = false;
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

    public void OnPause()
    {
        GameManager.Instance.GamePauseToggle();
    }
}
