using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RoleDescriptionConfig", menuName = "Config/RoleDescriptionConfig", order = 0)]
public class RoleDescriptionConfig : ScriptableObject
{
    public List<RoleDetail> details;
    public RoleDetail GetDeatilByIndex(int index)
    {
        return details.First(x=>x.Index == index);
    }
}
[System.Serializable]
public class RoleDetail
{
    public int Index;
    public string Name;
    public string Description;
    public Sprite headIcon;
    public Sprite ClassIcon;
}
