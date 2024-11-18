using System;
using UnityEngine;

public class BurningBuff : MonoBehaviour
{
    Role owner;
    public float damage = 5;
    public float lifeTime = 2;
    public float tickTime = 1;
    private float lifeTimer = 0;
    public float tickTimer = 0;
    public void OnEnable()
    {
        owner = this.GetComponent<Role>();
        owner.ReceiveDamage(damage);
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer > tickTime)
        {
            tickTimer -= tickTime;
            Tick();
        }

        if (lifeTimer > lifeTime)
        {
            GameObject.Destroy(this);
        }
    }

    private void Tick()
    {
        owner.ReceiveDamage(damage);
    }

    public void ResetLifeTimer()
    {
        lifeTimer = 0;
    }
}
