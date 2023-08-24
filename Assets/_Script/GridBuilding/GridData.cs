using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GridData // this is virtual grid to check where can you place element
{
    Dictionary<Vector3Int, PlacementData> placementElement = new();

    public void SetGridSize(Vector2Int gridSizeInAxis)
    {
        Vector2Int xAxis = new Vector2Int(CalculateCoordinates(gridSizeInAxis.x).plus, CalculateCoordinates(gridSizeInAxis.x).minus);
        Vector2Int yAxis = new Vector2Int(CalculateCoordinates(gridSizeInAxis.y).plus, CalculateCoordinates(gridSizeInAxis.y).minus);

        for (int x = xAxis.y; x <= xAxis.x; x++)
        {
            AddElementOnGridPosition(new Vector3Int(x, 0, yAxis.x), new Vector2Int(1, 1), -1, -1, false);
            AddElementOnGridPosition(new Vector3Int(x, 0, yAxis.y), new Vector2Int(1, 1), -1, -1, false);
        }
        for (int y = yAxis.y; y <= yAxis.x; y++)
        {
            AddElementOnGridPosition(new Vector3Int(xAxis.x, 0, y), new Vector2Int(1, 1), -1, -1, false);
            AddElementOnGridPosition(new Vector3Int(xAxis.y, 0, y), new Vector2Int(1, 1), -1, -1, false);
        }
    }
    public void AddElementOnGridPosition(Vector3Int gridPosition, Vector2Int elementSize, int ID, int placementObjectIndex, bool canSetElementAtThis)
    {
        List<Vector3Int> occupatePosition = CalculatePosition(gridPosition, elementSize);
        PlacementData data = new PlacementData(occupatePosition, ID, placementObjectIndex, canSetElementAtThis);
        foreach (var position in occupatePosition)
        {
            placementElement[position] = data;
        }
    }
    public bool CanPlaceObjectAtThisCell(Vector3Int gridPosition, Vector2Int elementSize, bool elementCanBePlaceOnOther = true, GameObject prefab = null)
    {
        List<Vector3Int> occupatePosition = CalculatePosition(gridPosition, elementSize);
        foreach (var position in occupatePosition)
        {
            if (placementElement.ContainsKey(position))
            {
                return false;
            }
        }
        if (prefab != null)
        {
            if(prefab.GetComponent<ObjectRayCaster>().CheckRayCollision())
            {
                return true;
            }
            {
                if(elementCanBePlaceOnOther)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        Debug.Log("placement false");
        return true;


        /*List<Vector3Int> occupatePosition = CalculatePosition(gridPosition, elementSize);
        foreach (var position in occupatePosition)
        {
            if (placementElement.ContainsKey(position))
            {
                return false;
            }
        }
        // Do poprawy ale to kiedyś (jak nie zapomne)
        LayerMask elementGridLayer = 1 << 9;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,Mathf.Infinity, elementGridLayer))
        {
            if(elementCanBePlaceOnOther)
            {
                return true;
            }
            return false;
        }
        return true;*/
    }
    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int elementSize) //object Can Be Bigger Than 1x1 sometimes we must block more space
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < elementSize.x; x++)
        {
            for (int y = 0; y < elementSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }
    private (int plus, int minus) CalculateCoordinates(int value)
    {
        int plus;
        int minus;
        if (value % 2 == 0)
        {
            plus = value / 2;
            minus = -value / 2 - 1;
        }
        else
        {
            plus = Mathf.CeilToInt(value / 2) + 1;
            minus = -Mathf.CeilToInt(value / 2) - 1;
        }
        return (plus: plus, minus: minus);
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placementElement.ContainsKey(gridPosition) == false)
        { return -2; }
        return placementElement[gridPosition].placedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placementElement[gridPosition].occupiedPositions)
        {
            Debug.Log("tak");
            placementElement.Remove(pos);
        }
    }
}
public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int placedObjectIndex { get; private set; }
    public bool canSetElementAtThis { get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int ID, int placedObjectIndex, bool canSetElementAtThis)
    {
        this.ID = ID;
        this.occupiedPositions = occupiedPositions;
        this.placedObjectIndex = placedObjectIndex;
        this.canSetElementAtThis = canSetElementAtThis;
    }
}
