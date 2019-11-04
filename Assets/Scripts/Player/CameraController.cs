using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1;

    [Range(0, 1), SerializeField]
    private float zoomPercent;
    [SerializeField]
    private float zoomSpeed = 10;
    [SerializeField]
    private float zoomSensitivity = 1;

    void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        zoomPercent += scroll * zoomSensitivity;
        if (zoomPercent > 1)
            zoomPercent = 1;
        else if (zoomPercent < 0)
            zoomPercent = 0;

        transform.position = new Vector3(transform.position.x + dir.x, Mathf.Clamp(13 - (13 * zoomPercent), 3, 13), transform.position.z + dir.y);

            Vector3 targetRotation;
        if (zoomPercent > .6f)
        {
            float rotatePercent = 1 - zoomPercent;
            targetRotation = new Vector3(Mathf.Clamp(80 - (80 * zoomPercent) / 2, 40, 80), 0);
        }
        else
        {
            targetRotation = new Vector3(80, 0);
        }

            Vector3 rotation = Vector3.Lerp(transform.eulerAngles, targetRotation, zoomSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = rotation;

    }
}