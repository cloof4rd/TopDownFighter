using UnityEngine;

public class SwingSound : MonoBehaviour
{
    private void Awake()
    {
        AudioBinding.Instance.PlaySwingSound(this.transform.position);
    }
}
