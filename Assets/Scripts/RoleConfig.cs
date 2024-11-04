using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YogiGameCore.Utils;

[CreateAssetMenu(fileName = "roleConfig", menuName = "RoleConfig", order = 1)]
public class RoleConfig : ScriptableObject
{
    public string roleName;
    public string artPath;
    public float moveSpeed;

    public AnimSpeedConfig animSpeedConfig;
}

[System.Serializable]
public class AnimSpeedConfig
{
    public List<AnimSpeedPair> data;
    public float GetSpeedByAnimName(string name)
    {
        AnimSpeedPair result = data.FirstOrDefault(x => x.animName.Equals(name));
        if (result == null)
            return 1;
        return result.animSpeed;
    }
    public AnimSpeedPair GetConfigByName(string animName)
    {
        AnimSpeedPair result = data.FirstOrDefault(x => x.animName.Equals(animName));
        return result;
    }
}
[System.Serializable]
public class AnimSpeedPair
{
    public string animName;
    public float animSpeed = 1;

    public List<FXConfig> fxs;
    public List<BulletConfig> bullets;
}
[System.Serializable]
public class FXConfig
{
    [System.NonSerialized, HideInInspector]
    public FX prefab;

    [Range(-1, 14)]
    public int spawnFrameIndex;
    public string fxName;
    // the offset of the orientaion based on the role
    // x: role Forward Distance,y: role Right Distance
    public Vector2 fxPosition;
    public float lifeTime = 1.0f;
    public bool isDestroyWhenAnimOver = true;
    // Movement
    public bool isMove = false;
    public float moveSpeed = 1.0f;


    public void Init()
    {
        if (!fxName.IsNotNullAndEmpty() || prefab != null)
            return;
        this.prefab = Resources.Load<FX>(this.fxName);
        if (prefab != null)
            return;
        Debug.LogError($"Error Load Path:{fxName} or Prefab Has not 'FX' script attached");
    }
}
[System.Serializable]
public class BulletConfig
{
    [System.NonSerialized, HideInInspector]
    public Bullet prefab;
    [NonSerialized, HideInInspector]
    public int hitLayerMask;

    // Spawn
    [Range(-1, 14)]
    public int spawnFrameIndex;
    public string bulletName;
    // the offset of the orientaion based on the role
    // x: role Forward Distance,y: role Right Distance
    public Vector2 bulletPosition;
    public float lifeTime = 1.0f;
    public bool isDestroyWhenAnimOver = true;

    // Select
    public BulletSelectType selectType;
    public float radius = 1.0f;
    public float sectorAngle = 0f;

    // Damage
    public float damage = 1;

    // Movement
    public bool isMove = false;
    public float moveSpeed = 1.0f;

    //Sound
    [Obsolete]
    public bool isPlaySwingSound = false;

    public void Init()
    {
        hitLayerMask = ConstConfig.LayerMaskPlayer;
        if (this.bulletName.IsNotNullAndEmpty() && this.prefab == null)
            this.prefab = Resources.Load<Bullet>(this.bulletName);
    }
}
public enum TargetType
{
    Enemy,
    Self
}