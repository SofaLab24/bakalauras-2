using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    //private Vector3 _origin;
    //private Vector3 _difference;

    //private Camera _mainCamera;

    //private bool _isDragging;

    //private void Awake()
    //{
    //    _mainCamera = Camera.main;
    //}

    //public void OnDrag(InputAction.CallbackContext ctx)
    //{
    //    Debug.Log("Drag works");
    //    if (ctx.started) _origin = GetMousePosition;
    //    _isDragging = ctx.started || ctx.performed;
    //}

    //private void LateUpdate()
    //{
    //    Debug.Log("Mouse position: " + GetMousePosition);
    //    if (!_isDragging) return;
    //    _difference = GetMousePosition - transform.position;
    //    Debug.Log("Difference: " + _difference);
    //    transform.position = _origin - _difference;
    //}

    //private Vector3 GetMousePositionPreset => _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    //private Vector3 GetMousePosition => new Vector3(GetMousePositionPreset.x, 40, GetMousePositionPreset.y);


}
