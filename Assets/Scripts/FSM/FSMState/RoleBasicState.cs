using System.Collections.Generic;
using UnityEngine;

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
}
