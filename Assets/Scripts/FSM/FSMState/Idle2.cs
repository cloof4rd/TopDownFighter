
/// <summary>
/// Idle2 Used For Block Attack
/// </summary>
public class Idle2 : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        role.isBlockBullet = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (IsEndAnim())
            stateMachine.Change<Idle>();
    }

    public override void OnExit()
    {
        base.OnExit();
        role.isBlockBullet = false;
    }
}
