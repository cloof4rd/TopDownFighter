using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Utils;

public class RoleBasicState : YogiGameCore.FSM.FSMState
{
    protected Role role;
    protected RoleFSMStateMachine stateMachine => role.animFSM;
    protected float timer = 0;
    float animFrameTimer = 0;
    int animFrameIndex = 0;
    Dictionary<Direction, Sprite[]> animData;
    protected bool isLoop = false;
    protected AnimSpeedPair animConfig => role.config.animSpeedConfig.GetConfigByName(this.GetType().Name);
    protected float animConfigSpeed => animConfig == null ? 1 : animConfig.animSpeed;
    public virtual void Init(Role role)
    {
        this.role = role;
        //animConfigSpeed = role.animSpeedConfig.GetSpeedByAnimName(this.GetType().Name);
        InitAnimData(this.GetType().Name);
    }
    public void InitAnimData(string animName)
    {
        if (animData == null)
            animData = SpritesLoader.LoadSprites($"{role.config.artPath}/{animName}");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        timer = animFrameTimer = animFrameIndex = 0;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        timer += Time.deltaTime;
        animFrameTimer += Time.deltaTime * role.frameCountPerSeconed * animConfigSpeed;
        if (isLoop)
        {
            animFrameIndex = (int)(animFrameTimer % ConstConfig.ArtRoleSpriteColumnCount);
        }
        else
        {
            if (animFrameTimer > ConstConfig.ArtRoleSpriteColumnCount - 1)
                animFrameIndex = ConstConfig.ArtRoleSpriteColumnCount - 1;
            else
                animFrameIndex = (int)(animFrameTimer);
        }
        role.spriteRenderer.sprite = animData[role.direction][animFrameIndex];
    }
    protected bool IsEndAnim()
    {
        return (animFrameIndex == ConstConfig.ArtRoleSpriteColumnCount - 1);
    }
    public override void OnExit()
    {
        timer = animFrameTimer = animFrameIndex = 0;
        base.OnExit();
    }

    #region Bullet
    List<BulletConfig> bullets;
    List<Bullet> spawnedBullets = new List<Bullet>();

    List<FXConfig> fxs;
    List<FX> spawnedFXs = new List<FX>();
    protected void BulletInit()
    {
        isLoop = false;
        if (animConfig == null)
            return;
        for (int i = 0; i < animConfig.bullets.Count; i++)
        {
            BulletConfig bulletConfig = animConfig.bullets[i];
            bulletConfig.Init();

        }
        for (int i = 0; i < animConfig.fxs.Count; i++)
        {
            var fx = animConfig.fxs[i];
            fx.Init();
        }
    }
    protected void BulletEnter()
    {
        this.role.isAttacking = true;
        bullets = new List<BulletConfig>();
        fxs = new List<FXConfig>();
        if (animConfig == null)
            return;
        bullets.AddRange(animConfig.bullets);
        fxs.AddRange(animConfig.fxs);
    }
    protected void BulletUpdate()
    {
        if (bullets != null)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                BulletConfig bulletConfig = bullets[i];
                if (bulletConfig.spawnFrameIndex > animFrameIndex)
                    continue;
                if (bulletConfig.prefab == null)
                    continue;
                Bullet bullet = GameObject.Instantiate(bulletConfig.prefab, this.role.transform);
                var forward = this.role.getForward;
                var right = Quaternion.Euler(0, 0, -90) * forward;
                var offset = bulletConfig.bulletPosition.y * (Vector3)role.getForward + bulletConfig.bulletPosition.x * right;
                bullet.transform.position += offset;
                bullet.transform.up = role.getForward;
                bullet.Init(role, bulletConfig);
                spawnedBullets.Add(bullet);

                bullets.RemoveAt(i);
                i--;
            }
        }

        if (fxs != null)
        {
            for (int i = 0; i < fxs.Count; i++)
            {
                var fxConfig = fxs[i];
                if (fxConfig.spawnFrameIndex > animFrameIndex)
                    continue;
                var fx = GameObject.Instantiate<FX>(fxConfig.prefab, this.role.transform);
                var forward = this.role.getForward;
                var right = Quaternion.Euler(0, 0, -90) * forward;
                var offset = fxConfig.fxPosition.y * (Vector3)role.getForward + fxConfig.fxPosition.x * right;
                fx.transform.position += offset;
                fx.transform.up = role.getForward;
                spawnedFXs.Add(fx);
                fx.Init(fxConfig);

                fxs.RemoveAt(i);
                i--;
            }

        }
        if (IsEndAnim())
            stateMachine.Change<Idle>();
    }
    protected void BulletExit()
    {
        for (int i = 0; i < spawnedBullets.Count; i++)
        {
            var bullet = spawnedBullets[i];
            if (bullet && bullet.isDestroyWhenAnimOver)
                bullet.Kill();
        }
        spawnedBullets.Clear();

        for (int i = 0; i < spawnedFXs.Count; i++)
        {
            var fx = spawnedFXs[i];
            if (fx && fx.isDestroyWhenAnimOver)
            {
                fx.Kill();
            }
        }

        this.role.isAttacking = false;
    }
    #endregion
}
