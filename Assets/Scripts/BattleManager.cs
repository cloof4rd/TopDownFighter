using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public List<BattleInfo> infoArr;

    public Action<List<BattleInfo>> OnFinishBattle;
    public void Awake()
    {
        Instance = this;
        infoArr = new List<BattleInfo>();
    }
    public void InitRoleBattleInfo(Role role, int playerIndex)
    {
        var info = new BattleInfo(playerIndex, role.roleIndex);
        infoArr.Add(info);
        role.OnBeHit += () => info.beHitCount++;
        role.OnTryBlock += () => info.tryToBlockCount++;
        role.OnBlockSuccess += () => info.blockSuccessCount++;
        role.OnDie += () => info.isWinner = false;
    }
    public void FinishBattle()
    {
        OnFinishBattle?.Invoke(infoArr);
    }
}

public class BattleInfo
{
    public int playerIndex;
    public int roleIndex;
    public int beHitCount;
    public int tryToBlockCount;// block anim count
    public int blockSuccessCount;// block success count
    public bool isWinner = true;
    public BattleInfo(int index, int roleIndex)
    {
        playerIndex = index;
        this.roleIndex = roleIndex;
    }
}
