using UnityEngine;

[System.Serializable]
public class InputData
{
    public Vector2 moveDir;
    public bool isMoveing;
    public bool isAttack1;
    public bool isAttack2;
    public bool isAttack3;
    public bool isAttack4;
    public bool isBlockSkill;

    public void Clear()
    {
        moveDir = Vector2.zero;
        isMoveing = false;
        isAttack1 = false;
        isAttack2 = false;
        isAttack3 = false;
        isAttack4 = false;
        isBlockSkill = false;
    }
}
