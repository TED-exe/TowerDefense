using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlacementState : IBuildingState
{
    private int selectedElementIndex = -1;
    private float rotateValue;
    private GameObject elementPreview;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ElementsDatabase database;
    GridData data;
    ElementPlacer elementPlacer;
    ElementRotateSystem rotateSystem;

    public PlacementState(
        int iD,
        Grid grid,
        PreviewSystem previewSystem,
        ElementsDatabase database,
        GridData data,
        ElementPlacer elementPlacer,
        ElementRotateSystem rotateSystem)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.data = data;
        this.elementPlacer = elementPlacer;

        selectedElementIndex = database.element.FindIndex(x => x.ID == iD); // select element
        if (selectedElementIndex > -1)
        { elementPreview = previewSystem.StartShowingPlacementPreview(
            database.element[selectedElementIndex].elementPrefab,
            database.element[selectedElementIndex].elementSize); }
        else
        { throw new System.Exception($"No object With ID {iD}"); }
        this.rotateSystem = rotateSystem;
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
        rotateValue = 0;
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = data.CanPlaceObjectAtThisCell(
            gridPosition,
            database.element[selectedElementIndex].elementSize,
            database.element[selectedElementIndex].canByPlacedOnOtherElemets);


        if (!placementValidity)
        { return; }
        else
        {
            if (!elementPreview.GetComponent<ElementRayCaster>().CheckRayCollision())
            {
                if (database.element[selectedElementIndex].canByPlacedOnOtherElemets)
                { PlaceElement(gridPosition); }

                return;
            }
            else
            { PlaceElement(gridPosition); }
        }
    }
    private void PlaceElement(Vector3Int gridPosition)
    {
        int index = elementPlacer.PlaceElement(
            database.element[selectedElementIndex].elementPrefab,
            grid.CellToWorld(gridPosition), rotateValue);

        data.AddElementOnGridPosition(
            gridPosition,
            database.element[selectedElementIndex].elementSize,
            database.element[selectedElementIndex].ID,
            index,
            database.element[selectedElementIndex].canPlaceOnThisElement);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
    public void UpdateState(Vector3Int gridPosition, Vector3 cellWorldPosition)
    {
        if (Input.GetKeyDown(KeyCode.E))
        { rotateValue += rotateSystem.RotateElement(elementPreview, true); }
        if (Input.GetKeyDown(KeyCode.Q))
        { rotateValue += rotateSystem.RotateElement(elementPreview, false); }

        bool placementValidity = data.CanPlaceObjectAtThisCell(
            gridPosition,
            database.element[selectedElementIndex].elementSize,
            database.element[selectedElementIndex].canByPlacedOnOtherElemets);

        previewSystem.UpdatePosition(cellWorldPosition, placementValidity);
        if (placementValidity && !elementPreview.GetComponent<ElementRayCaster>().CheckRayCollision())
        {
            if (database.element[selectedElementIndex].canByPlacedOnOtherElemets)
            { previewSystem.UpdatePosition(cellWorldPosition, true); }
            else
            { previewSystem.UpdatePosition(cellWorldPosition, false); }
        }
    }
}
