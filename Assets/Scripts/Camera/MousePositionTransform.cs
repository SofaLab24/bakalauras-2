using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePositionTransform : MonoBehaviour
{
    Vector3 mPosition;
 
    [SerializeField]
    float sensitivity = 0.3f;
    [SerializeField]
    float moveBorder = 50f;
    [SerializeField]
    float moveSpeed = 3f;
    [SerializeField]
    float zoomSpeed = 500f;
    [SerializeField]
    float maxZoomOut = 100f;
    private void Update()
    {
        // XZ movement
        mPosition = Input.mousePosition;
        if (mPosition.x < moveBorder && mPosition.x >= 0)
        {
            transform.position = new Vector3(transform.position.x - moveBorder * Time.deltaTime * moveSpeed, transform.position.y, transform.position.z);
        }
        if (mPosition.y < moveBorder && mPosition.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveBorder * Time.deltaTime * moveSpeed);
        }
        if (mPosition.x > Screen.width - moveBorder && mPosition.x <= Screen.width)
        {
            transform.position = new Vector3(transform.position.x + moveBorder * Time.deltaTime * moveSpeed, transform.position.y, transform.position.z);
        }
        if (mPosition.y > Screen.height - moveBorder && mPosition.y <= Screen.height)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveBorder * Time.deltaTime * moveSpeed);
        }

        // Y movement
        if (Mouse.current.scroll.ReadValue().y > 0 && transform.position.y >= 0) // zoom in
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime * zoomSpeed, transform.position.z);
        }
        else if (Mouse.current.scroll.ReadValue().y < 0 && transform.position.y <= maxZoomOut) // zoom out
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * zoomSpeed, transform.position.z);
        }
    }
}
