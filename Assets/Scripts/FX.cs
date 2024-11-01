using System;
using UnityEngine;
using YogiGameCore.Utils;

public class FX : MonoBehaviour
{
    [NonSerialized, HideInInspector]
    public FXConfig config;
    private float lifeTimer;

    public float lifeTime => config.lifeTime;

    // Life
    public bool isDestroyWhenAnimOver => config.isDestroyWhenAnimOver;

    public void Init(FXConfig fxConfig)
    {
        this.config = fxConfig;
        lifeTimer = lifeTime;
        if (config.isMove)
        {
            var move = this.gameObject.AddComponent<Move>();
            move.speed = config.moveSpeed;
        }
    }
    private void Update()
    {
        if (this.config == null)
            return;
        lifeTimer -= Time.deltaTime;
        if (lifeTimer > 0.0f)
            return;
        Kill();
    }
    public void Kill()
    {
        gameObject.DestroySelf();
    }
}
