using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 pivot;
    public Vector3 offset;

    Vector3 velo;

       // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Vector3 localRight = Vector3.Cross(Vector3.up, offset);
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up)
                      * Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), localRight)
                      * offset;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
                             target.position + pivot + offset, ref velo,
                             0.5f, 20f, Time.fixedDeltaTime);
        transform.forward = target.position - transform.position;
    }
}
