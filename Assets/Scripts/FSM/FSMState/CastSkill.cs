using UnityEngine;
using YogiGameCore.Utils;

public abstract class CastSkill : RoleBasicState
{
    Bullet bulletPrefab;
    Bullet bullet;
    public override void Init(Role role)
    {
        base.Init(role);
        isLoop = false;
        if (animConfig.bulletName.IsNotNullAndEmpty())
            bulletPrefab = Resources.Load<Bullet>(animConfig.bulletName);
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (bullet == null && timer > animConfig.bulletSpawnTime && bulletPrefab != null)
        {
            bullet = GameObject.Instantiate(bulletPrefab, this.role.transform);
            bullet.Init(role);
        }

        if (IsEndAnim())
            stateMachine.Change<Idle>();
    }
    public override void OnExit()
    {
        base.OnExit();
        if (bullet && bullet.isDestroyWhenAnimOver)
            bullet.Kill();
        bullet = null;
    }
}
