using DTT.AreaOfEffectRegions;
using System;
using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Utils;

public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public Role owner;
    [NonSerialized, HideInInspector]
    public BulletConfig config;
    private HashSet<Role> alreadyHit = new HashSet<Role>();
    private float lifeTimer;

    // Life
    public bool isDestroyWhenAnimOver => config.isDestroyWhenAnimOver;
    public float lifeTime => config.lifeTime;
    // Select
    public int hitLayerMask => config.hitLayerMask;
    public BulletSelectType selectType => config.selectType;
    public float radius => config.radius;
    public float sectorAngle => config.sectorAngle;
    // Damage
    public float damage => config.damage;

    public void Init(Role owner, BulletConfig bulletConfig)
    {
        this.owner = owner;
        this.config = bulletConfig;
        lifeTimer = lifeTime;
    }
    private void Update()
    {

        switch (selectType)
        {
            case BulletSelectType.Sphere:
                SphereUpdate();
                break;
            case BulletSelectType.Sector:
                SectorUpdate();
                break;
            default:
                break;
        }
        lifeTimer -= Time.deltaTime;
        if (lifeTimer > 0.0f)
            return;
        Kill();
    }
    float debugDuration = 0.1f;
    void SphereUpdate()
    {
        Collider2D[] hits = Select2D.SphereSelect(this.transform.position, radius, hitLayerMask);
        Hit(hits);
        DebugEx.DrawCircle(this.transform.position, radius, Color.red, debugDuration, DebugEx.Panel.XY, .1f);
    }
    void SectorUpdate()
    {
        Collider2D[] hits = Select2D.SectorSelect(this.transform.position, this.transform.up, radius, sectorAngle, hitLayerMask);
        Hit(hits);
        DebugEx.DrawSector(this.transform.position, owner.getForward, sectorAngle, radius, debugDuration, DebugEx.Panel.XY, 90.0f);
    }

    void Hit(Collider2D[] hits)
    {
        HashSet<Role> roles = new HashSet<Role>();
        foreach (var hit in hits)
        {
            var target = hit.GetComponent<Role>();
            if (target == owner || roles.Contains(target))
                continue;
            roles.Add(target);
        }
        foreach (var target in roles)
        {
            if (alreadyHit.Contains(target))
                continue;
            alreadyHit.Add(target);
            if (target != null && target != owner)
                target.ReceiveDamage(damage);
        }
    }

    private void OnEnterArea(Collider2D obj)
    {

    }
    private void OnStayArea(Collider2D obj)
    {
        var target = obj.GetComponent<Role>();
        if (alreadyHit.Contains(target))
            return;
        alreadyHit.Add(target);
        if (target != null && target != owner)
            target.ReceiveDamage(damage);
    }
    private void OnExitArea(Collider2D obj)
    {
    }
    public void Kill()
    {
        this.gameObject.DestroySelf();
    }
}
public enum BulletSelectType
{
    Sphere,
    Sector,
}

public static class Select2D
{
    public static Collider2D[] SphereSelect(Vector2 point, float radius, int layerMask)
    {
        return Physics2D.OverlapCircleAll(point, radius, layerMask);
    }
    public static Collider2D[] SectorSelect(Vector2 point, Vector2 forward, float radius, float angle, int layerMask)
    {
        var allTarget = SphereSelect(point, radius, layerMask);
        List<Collider2D> result = new List<Collider2D>();
        foreach (var target in allTarget)
        {
            var dir = (Vector2)target.transform.position - point;
            if (Vector2.Angle(forward, dir) < (angle / 2.0f))
            {
                result.Add(target);
            }
        }
        return result.ToArray();
    }
    public static Collider2D[] BoxSelect(Vector2 point, Vector2 size, float angle, int layerMaxk)
    {
        return Physics2D.OverlapBoxAll(point, size, angle, layerMaxk);
    }
}