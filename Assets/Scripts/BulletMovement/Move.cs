using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1.0f;

    private void Start()
    {
        this.transform.SetParent(null);
    }
    public void Update()
    {
        this.transform.position += this.transform.up * speed * Time.deltaTime;
    }
}
