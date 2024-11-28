using System.Threading.Tasks;
using UnityEngine;
using YogiGameCore.Utils;

public class Idle : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = true;

        var animConfig = role.config.animSpeedConfig;
        animConfig.data.ForEach(data =>
        {
            var skillName = data.animName;
            if (data.SkillInputKey.IsNullOrEmpty())
                return;
            role.AddSkill(data.SkillInputKey, async () =>
            {
                switch (skillName)
                {
                    case "Attack1":
                        role.inputData.isAttack1 = true;
                        await Task.Yield();
                        role.inputData.isAttack1 = false;
                        break;
                    case "Attack2":
                        role.inputData.isAttack2 = true;
                        await Task.Yield();
                        role.inputData.isAttack2 = false;
                        break;
                    case "Attack3":
                        role.inputData.isAttack3 = true;
                        await Task.Yield();
                        role.inputData.isAttack3 = false;
                        break;
                    case "Attack4":
                        role.inputData.isAttack4 = true;
                        await Task.Yield();
                        role.inputData.isAttack4 = false;
                        break;
                    default:
                        break;
                }
            });
        });

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (role.inputData.isMoveing && role.isCanMove)
            stateMachine.Change<Run>();

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
