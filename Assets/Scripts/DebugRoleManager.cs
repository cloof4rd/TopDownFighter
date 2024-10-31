using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugRoleManager : MonoBehaviour
{
    public Role rolePrefab;

    public float gapY = .5f;

    public float gapX = .5f;

    public List<List<Role>> roles = new List<List<Role>>();
    public TextMeshPro textProPrefab;
    private void Awake()
    {
        int columnCount = 8;// idle idle2 run takeDamage  attack1 attack2 attack3 attack4
        for (int x = 0; x < columnCount; x++) // for anim count
        {
            var row = new List<Role>();
            roles.Add(row);
            for (int y = ConstConfig.RoleMinMaxIndex.x; y < ConstConfig.RoleMinMaxIndex.y; y++) // for role type count
            {
                row.Add(SpawnRole(new Vector2(x * gapX, y * gapY), y));
            }
        }
        SetInputData();
    }
    private void SetInputData()
    {
        SpawnDisplayName("Idle", 0);

        SpawnDisplayName("Idle2", 1);
        roles[1].ForEach(x => x.inputData.isBlockSkill = true);

        SpawnDisplayName("Run", 2);
        roles[2].ForEach(x =>
        {
            x.inputData.moveDir = new Vector2(0, 0);
            x.inputData.isMoveing = true;
        });

        IEnumerator DelayToDamage()
        {
            while (true)
            {
                var frameLength = roles[3][0].frameCountPerSeconed;
                var length = ConstConfig.ArtRoleSpriteColumnCount / frameLength;
                yield return new WaitForSeconds(length+0.1f);
                roles[3].ForEach(x => x.ReceiveDamage(0));
            }
        }
        StartCoroutine(DelayToDamage());

        SpawnDisplayName("TakeDamage", 3);


        SpawnDisplayName("Attack1", 4);
        roles[4].ForEach(x => x.inputData.isAttack1 = true);

        SpawnDisplayName("Attack2", 5);
        roles[5].ForEach(x => x.inputData.isAttack2 = true);

        SpawnDisplayName("Attack3", 6);
        roles[6].ForEach(x => x.inputData.isAttack3 = true);

        SpawnDisplayName("Attack4", 7);
        roles[7].ForEach(x => x.inputData.isAttack4 = true);
    }
    private void Update()
    {
    }


    private void SpawnDisplayName(string displayName, int columnIndex)
    {
        var go = GameObject.Instantiate(textProPrefab, roles[columnIndex][0].transform.position - new Vector3(0, gapX, 0), Quaternion.identity);
        go.text = displayName;
        go.gameObject.SetActive(true);
    }

    private Role SpawnRole(Vector2 pos, int roleIndex)
    {
        var role = GameObject.Instantiate<Role>(rolePrefab);
        role.Init(roleIndex);
        role.transform.position = pos;
        pos.y += gapY;
        role.gameObject.SetActive(true);
        return role;
    }
}
