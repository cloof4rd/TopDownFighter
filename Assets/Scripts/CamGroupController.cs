using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Utils.MonoExtent;

public class CamGroupController : MonoBehaviour
{
    CinemachineTargetGroup group;
    private void Awake()
    {
        group = GetComponent<CinemachineTargetGroup>();
    }

    [Button]
    public async UniTask OnlyShowSingle(int index, float animTime)
    {
        while (animTime > 0)
        {
            animTime -= Time.fixedDeltaTime;

            for (int i = 0; i < group.m_Targets.Length; i++)
            {
                if (i == index)
                    continue;
                CinemachineTargetGroup.Target target = group.m_Targets[i];
                target.weight -= Time.deltaTime;
                group.m_Targets[i] = target;
            }
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }
}
