using System.Collections.Generic;
using UnityEngine;
using YogiGameCore.Utils;

public class Bullet : MonoBehaviour
{
    public float damage = 1;
    public bool isDestroyWhenAnimOver = true;
    public List<Collider2DListener> listeners;

    [HideInInspector]
    public Role owner;

    private HashSet<Role> alreadyHit = new HashSet<Role>();

    public void Init(Role owner)
    {
        this.owner = owner;
    }
    private void Awake()
    {
        foreach (var listener in listeners)
        {
            listener.OnTriggerEnter += OnEnterArea;
            listener.OnTriggerStay += OnStayArea;
            listener.OnTriggerExit += OnExitArea;
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
