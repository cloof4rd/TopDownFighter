using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Utils.MonoExtent;

public class Test : MonoBehaviour
{
    public DynamicTextData data;
    [Button]
    void ABC()
    {
        DynamicTextManager.CreateText2D(this.transform.position, "Test", data);
    }
}
