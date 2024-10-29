using UnityEngine;

public class Idle : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (role.inputData.isMoveing && role.isCanMove)
            stateMachine.Change<Run>();
        if (role.inputData.isAttack1)
            stateMachine.Change<Attack1>();
        if (role.inputData.isBlockSkill)
            stateMachine.Change<Idle2>();
    }
}
