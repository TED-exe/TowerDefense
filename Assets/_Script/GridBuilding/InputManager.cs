using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 lastMouseIndicatorPosition;

    public event Action onClick, onExit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { onClick?.Invoke(); }
        if (Input.GetKeyDown(KeyCode.Escape))
        { onExit?.Invoke(); }
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public (Vector3 position, bool setActive) GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, placementLayerMask))
        {
            lastMouseIndicatorPosition = hit.point;
            return (position: lastMouseIndicatorPosition, setActive: true);
        }
        else
        { return (position: lastMouseIndicatorPosition, setActive: false); }
    }
}
