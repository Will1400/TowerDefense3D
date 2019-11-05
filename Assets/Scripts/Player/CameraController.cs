using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10;

    [Range(0, 1), SerializeField]
    private float zoomPercent;
    [SerializeField]
    private float zoomSpeed = 10;
    [SerializeField]
    private float zoomSensitivity = 1;

    void Update()
    {
        // Movement
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        transform.Translate(((Vector3.right * dir.x) + (Vector3.forward * dir.y)) * Time.deltaTime, Space.World);

        Vector2 mapSize = new Vector2(MapGenerator.Instance.Map.GetUpperBound(0), MapGenerator.Instance.Map.GetUpperBound(1)) + Vector2.one;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -(mapSize.x / 2), mapSize.x / 2), transform.position.y, Mathf.Clamp(transform.position.z, -(mapSize.y / 2), mapSize.y / 2));


        // Zoom
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        zoomPercent += scroll * zoomSensitivity;
        if (zoomPercent > 1)
            zoomPercent = 1;
        else if (zoomPercent < 0)
            zoomPercent = 0;

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, Mathf.Clamp(16 - (13 * zoomPercent), 3, 16), transform.position.z), zoomSpeed * Time.deltaTime);

        // Zoom rotation
        Vector3 targetRotation;
        if (zoomPercent > .5f)
        {
            float rotatePercent = (zoomPercent - .5f) / .5f;
            targetRotation = new Vector3(Mathf.Clamp(80 - (80 * rotatePercent) / 2, 40, 80), 0);
        }
        else
        {
            targetRotation = new Vector3(80, 0);
        }

        Vector3 rotation = Vector3.Lerp(transform.eulerAngles, targetRotation, zoomSpeed * Time.deltaTime);
        transform.eulerAngles = rotation;
    }
}