public class TakeDamage : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = false;
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (IsEndAnim())
        {
            if (role.hp <= 0)
            {
                stateMachine.Change<Die>();
            }
            else
            {
                stateMachine.Change<Idle>();
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
