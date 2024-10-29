using UnityEngine;
using YogiGameCore.Utils.MonoExtent;

public class Role : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int roleIndex = 0;
    public RoleConfig config;

    [Header("Anim Direction")]
    public Direction direction = Direction.E;
    public int frameCountPerSeconed = 30;
    public RoleFSMStateMachine animFSM;


    public bool isCanMove = true;
    public InputData inputData = new InputData();

    public float hp = 100;
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

    public void Init(int index)
    {
        this.roleIndex = index;
        InitConfigByRoleIndex();
        animFSM = new RoleFSMStateMachine(this);
    }

    private void Update()
    {
        if (inputData.isMoveing)
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
            return;
        }
        if (isMissBullet)
        {
            PopupTextManager.PopupMiss(this.transform.position, "Miss");
            return;
        }
        PopupTextManager.PopupDamage(this.transform.position, damage.ToString());
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
        animFSM.Change<TakeDamage>();
    }
}

