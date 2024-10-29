using System;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(1, 1, 1);
    public float rotateSpeed = 1;
    private Vector3 lastPos;
    private void LateUpdate()
    {
        float delta = Time.deltaTime;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        if (Math.Abs(horizontal) > 0.01f || Math.Abs(vertical) > 0.01)
        {
            this.transform.position += (this.transform.right * horizontal * moveSpeed.x + this.transform.forward * vertical * moveSpeed.z) * delta;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            this.transform.position -= this.transform.up * delta * moveSpeed.y;
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.transform.position += this.transform.up * delta * moveSpeed.y;
        }
        
        if (Input.GetMouseButton(1))
        {
            var offset = Input.mousePosition - lastPos;
            offset = new Vector2(-offset.y, offset.x) * delta  * rotateSpeed;
            this.transform.eulerAngles += offset;
        }
        lastPos = Input.mousePosition;
    }
}
