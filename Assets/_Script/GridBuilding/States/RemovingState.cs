using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData data;
    ElementPlacer elementPlacer;


    public RemovingState(Grid grid, PreviewSystem previewSystem, GridData data, ElementPlacer elementPlacer)
    {

        this.grid = grid;
        this.previewSystem = previewSystem;
        this.data = data;
        this.elementPlacer = elementPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    // dodac sprawdzenie czy na bloku coś stoi (jeśli tak to nie można usunąć tego na dole)
    public void OnAction(Vector3Int gridPosition)
    {
        if (!data.CanPlaceObjectAtThisCell(gridPosition, Vector2Int.one))
        {
            gameObjectIndex = data.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
            { return; }
            data.RemoveObjectAt(gridPosition);
            elementPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(data.CanPlaceObjectAtThisCell(gridPosition, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPosition, Vector3 cellWorldPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
