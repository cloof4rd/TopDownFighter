using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Role>(out var role))
        {
            if (role.TryGetComponent<BurningBuff>(out var buff))
            {
                buff.ResetLifeTimer();
            }
            else
            {
                role.AddComponent<BurningBuff>();
            }
        }
    }
}
