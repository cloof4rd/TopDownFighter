using System;
using UnityEngine;

public class Run : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!role.isCanMove || !role.inputData.isMoveing)
            stateMachine.Change<Idle>();

        if (role.inputData.isAttack1)
            stateMachine.Change<Attack1>();
        if (role.inputData.isAttack2)
            stateMachine.Change<Attack2>();
        if (role.inputData.isAttack3)
            stateMachine.Change<Attack3>();
        if (role.inputData.isAttack4)
            stateMachine.Change<Attack4>();
        if (role.inputData.isBlockSkill)
            stateMachine.Change<Idle2>();
    }
}
