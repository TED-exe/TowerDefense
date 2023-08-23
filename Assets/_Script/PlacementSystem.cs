using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("REFERENCE")]
    [SerializeField] private Transform elementsParent;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PreviewSystem previewSystem;
    [SerializeField] private Grid grid;
    [SerializeField] private ElementsDatabase elementDatabase;
    [SerializeField] private ElementPlacer elementPlacer;

    private GridData gridData = new();
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    IBuildingState buildingState;


    private void Awake()
    {
        float scaleX = Convert.ToSingle(gridVisualization.transform.localScale.x / 0.1);
        float scaleY = Convert.ToSingle(gridVisualization.transform.localScale.z / 0.1);
        StopPlacement();
        GridLayerOffset((int)scaleX, (int)scaleY);
        gridData.SetGridSize(new Vector2Int((int)scaleX, (int)scaleY));
    }
    private void Update()
    {
        if (buildingState == null)
            return;

        Vector3 mousePos = inputManager.GetMousePosition().position;
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        Vector3 cellWorldPosition = grid.CellToWorld(gridPos);
        cellWorldPosition += Vector3.up * 0.02f;

        if (lastDetectedPosition != gridPos)
        {
            buildingState.UpdateState(gridPos, cellWorldPosition);
            lastDetectedPosition = gridPos;
        }
    }
    private void GridLayerOffset(int scaleX, int scaleY)//give offset in position (gridLayer)
    {
        if (scaleX % 2 != 0)
        {
            gridVisualization.transform.localPosition += new Vector3(0.5f, 0, 0);
        }
        if (scaleY % 2 != 0)
        {
            gridVisualization.transform.localPosition += new Vector3(0, 0, 0.5f);
        }

    }
    public void StartPlacement(int elementIndex) // this method is called on ui button click;
    {
        StopPlacement(); // MUST BE STOPPED!!!! if you don't reset it, it will be triggered several times 
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(elementIndex, grid, previewSystem, elementDatabase, gridData, elementPlacer);

        // subscribe method in action
        inputManager.onClick += PlaceElement;
        inputManager.onExit += StopPlacement;
    }
    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, previewSystem, gridData, elementPlacer);
        inputManager.onClick += PlaceElement;
        inputManager.onExit += StopPlacement;
    }
    private void StopPlacement()
    {
        if (buildingState == null)
        { return; }
        gridVisualization.SetActive(false);
        buildingState.EndState();

        // unsubscibe method from action
        inputManager.onClick -= PlaceElement;
        inputManager.onExit -= StopPlacement;


        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
    private void PlaceElement()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetMousePosition().position;
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }
}
