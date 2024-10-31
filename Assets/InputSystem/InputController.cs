using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Role))]
public class InputController : MonoBehaviour
{
    PlayerInput input;
    public int playerIndex;
    private Role role;
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        playerIndex = input.playerIndex;

        role = GetComponent<Role>();
    }

    public void OnMove(InputValue v)
    {
        var data = role.inputData;
        data.moveDir = v.Get<Vector2>();
        data.isMoveing = data.moveDir.sqrMagnitude > 0.1f;
    }
    public async void OnAttack1()
    {
        role.inputData.isAttack1 = true;
        await Task.Yield();
        role.inputData.isAttack1 = false;
    }
    public async void OnAttack2()
    {
        role.inputData.isAttack2 = true;
        await Task.Yield();
        role.inputData.isAttack2 = false;
    }
    public async void OnAttack3()
    {
        role.inputData.isAttack3 = true;
        await Task.Yield();
        role.inputData.isAttack3 = false;
    }
    public async void OnAttack4()
    {
        role.inputData.isAttack4 = true;
        await Task.Yield();
        role.inputData.isAttack4 = false;
    }

    public async void OnBlockSkill()
    {
        role.inputData.isBlockSkill = true;
        await Task.Yield();
        role.inputData.isBlockSkill = false;
    }
}
