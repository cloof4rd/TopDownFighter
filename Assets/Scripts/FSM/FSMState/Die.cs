public class Die : RoleBasicState
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
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
