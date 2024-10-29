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
        if (!role.isCanMove)
            stateMachine.Change<Idle>();
        if (!role.inputData.isMoveing)
            stateMachine.Change<Idle>();
        if (role.inputData.isAttack1)
            stateMachine.Change<Attack1>();
        if (role.inputData.isBlockSkill)
            stateMachine.Change<Idle2>();
    }
}
