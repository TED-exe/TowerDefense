using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlacementState : IBuildingState
{
    private int selectedElementIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ElementsDatabase database;
    GridData data;
    ElementPlacer elementPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ElementsDatabase database, GridData data, ElementPlacer elementPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.data = data;
        this.elementPlacer = elementPlacer;

        selectedElementIndex = database.element.FindIndex(x => x.ID == iD); // select element
        if (selectedElementIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.element[selectedElementIndex].elementPrefab, database.element[selectedElementIndex].elementSize);
        }
        else
            throw new System.Exception($"No object With ID {iD}");
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool canPlaceElementOnThisCell = data.CanPlaceObjectAtThisCell(gridPosition, database.element[selectedElementIndex].elementSize).canPlace;
        bool canStackElementOnThisCell = data.CanPlaceObjectAtThisCell(gridPosition, database.element[selectedElementIndex].elementSize).canPlaceOnThisElement;

        if (!canPlaceElementOnThisCell && !canStackElementOnThisCell)
        { return; }
        else if (!canPlaceElementOnThisCell && canStackElementOnThisCell && database.element[selectedElementIndex].canSetElementAtThis == true)
        { return; }

        int index = elementPlacer.PlaceElement(database.element[selectedElementIndex].elementPrefab, grid.CellToWorld(gridPosition));

        data.AddElementOnGridPosition(gridPosition, database.element[selectedElementIndex].elementSize, database.element[selectedElementIndex].ID, index, database.element[selectedElementIndex].canSetElementAtThis);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition, Vector3 cellWorldPosition)
    {
        bool placementValidity;
        bool canPlaceElementOnThisCell = data.CanPlaceObjectAtThisCell(gridPosition, database.element[selectedElementIndex].elementSize).canPlace;
        bool canStackElementOnThisCell = data.CanPlaceObjectAtThisCell(gridPosition, database.element[selectedElementIndex].elementSize).canPlaceOnThisElement;


        if (!canPlaceElementOnThisCell && !canStackElementOnThisCell)
        { placementValidity = false; }
        else if (!canPlaceElementOnThisCell && canStackElementOnThisCell && database.element[selectedElementIndex].canSetElementAtThis == true)
        { placementValidity = false; }
        else
        { placementValidity = true; }

        previewSystem.UpdatePosition(cellWorldPosition, placementValidity);
    }
}
