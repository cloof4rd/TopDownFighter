using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public float bulletSpawnTime;
    public string bulletName;
}
