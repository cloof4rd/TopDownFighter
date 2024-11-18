using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using YogiGameCore.Utils.MonoExtent;

public class Role : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int playerIndex = -1;
    public int roleIndex = 0;
    public RoleConfig config;

    [Header("Anim Direction")]
    public Direction direction = Direction.E;
    public Vector2 getForward
    {
        get
        {
            switch (direction)
            {
                case Direction.E:
                    return new Vector2(1, 0);
                case Direction.SE:
                    return new Vector2(1, -1);
                case Direction.S:
                    return new Vector2(0, -1);
                case Direction.SW:
                    return new Vector2(-1, -1);
                case Direction.W:
                    return new Vector2(-1, 0);
                case Direction.NW:
                    return new Vector2(-1, 1);
                case Direction.N:
                    return new Vector2(0, 1);
                case Direction.NE:
                    return new Vector2(1, 1);
                default:
                    break;
            }
            return transform.forward;
        }
    }
    public Vector2 getRight
    {
        get
        {
            return Quaternion.Euler(0, 0, -90) * getForward;
        }
    }
    public int frameCountPerSeconed = 30;
    public RoleFSMStateMachine animFSM;


    public bool isCanMove = true;
    public bool isAttacking = false;
    public InputData inputData = new InputData();

    public static Action<Role> OnPlayerBeHit, OnPlayerBlocked;
    public Action OnBeHit, OnBlockSuccess, OnTryBlock, OnDie;

    public float hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }
    private float _hp = 100;
    public bool isBlockBullet, isMissBullet;
    public bool isDead = false;

    private void InitConfigByRoleIndex()
    {
        config = Resources.Load<RoleConfig>($"Configs/{roleIndex}");
    }

    private void OnValidate()
    {
        config = Resources.Load<RoleConfig>($"Configs/{roleIndex}");
        if (config != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = SpritesLoader.LoadSprites($"{config.artPath}/Idle")[direction][0];
        }
    }

    public async void Init(int index)
    {
        this.roleIndex = index;
        InitConfigByRoleIndex();
        await UniTask.Yield();
        animFSM = new RoleFSMStateMachine(this);
    }


    private void Update()
    {
        if (isDead)
            return;
        if (inputData.isMoveing && !isAttacking && isCanMove)
        {
            UpdateDirectionByInput();
            UpdateMovement();
        }

    }

    private void UpdateDirectionByInput()
    {
        var dir = inputData.moveDir;
        float p = 0.5f;
        if (dir.x > p)
        {
            if (dir.y > p)
                direction = Direction.NE;
            else if (dir.y < -p)
                direction = Direction.SE;
            else
                direction = Direction.E;
        }
        else if (dir.x < -p)
        {
            if (dir.y > p)
                direction = Direction.NW;
            else if (dir.y < -p)
                direction = Direction.SW;
            else
                direction = Direction.W;
        }
        else
        {
            if (dir.y > p)
                direction = Direction.N;
            else if (dir.y < -p)
                direction = Direction.S;
        }
    }

    private void UpdateMovement()
    {

        Vector2 dir = inputData.moveDir;
        var moveOffset = (Vector3)dir.normalized * Time.deltaTime * config.moveSpeed;
        // Enviroment Block
        if (Physics2D.Raycast(this.transform.position, dir.normalized, moveOffset.magnitude, ConstConfig.LayerMaskBlock))
            return;
        this.transform.position += moveOffset;
    }
    [Button]
    void Test1()
    {
        PopupTextManager.PopupBlock(this.transform.position, "Block");
    }
    [Button]
    void Test2()
    {
        PopupTextManager.PopupMiss(this.transform.position, "Miss");
    }

    public void ReceiveDamage(float damage)
    {
        if (isDead)
            return;

        // Block or Miss
        if (isBlockBullet)
        {
            PopupTextManager.PopupBlock(this.transform.position, "Block");
            OnPlayerBlocked?.Invoke(this);
            OnBlockSuccess?.Invoke();
            return;
        }
        if (isMissBullet)
        {
            PopupTextManager.PopupMiss(this.transform.position, "Miss");
            return;
        }
        OnPlayerBeHit?.Invoke(this);
        OnBeHit?.Invoke();
        PopupTextManager.PopupDamage(this.transform.position, damage.ToString());
        if (hp < damage)
            damage = hp;
        hp -= damage;
        var ipnut = this.GetComponent<InputController>();
        if (playerIndex != -1 && HealthSystem.Instance != null)
        {

            if (playerIndex == 0)
            {
                HealthSystem.Instance.TakeDamage(damage);
            }
            else
            {
                HealthSystem.Instance.UseMana(damage);
            }
        }
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            OnDie?.Invoke();
        }
        animFSM.Change<TakeDamage>();
    }
}

