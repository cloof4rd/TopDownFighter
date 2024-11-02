public class Attack3 : RoleBasicState
{
    public override void Init(Role role)
    {
        base.Init(role);
        BulletInit();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        BulletEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        BulletUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        BulletExit();
    }
}