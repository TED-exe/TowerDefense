using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPlacer : MonoBehaviour
{
    [SerializeField] private Transform elementsParent;
    [SerializeField] private List<GameObject> placedElements = new List<GameObject>();

    public int PlaceElement(GameObject elementPrefab, Vector3 elementPosition)
    {
        GameObject newElement = Instantiate(elementPrefab, elementsParent);
        newElement.transform.position = elementPosition;
        placedElements.Add(newElement);
        return placedElements.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if(placedElements.Count <= gameObjectIndex || placedElements[gameObjectIndex] == null)
        { return; }
        Destroy(placedElements[gameObjectIndex]);
        placedElements[gameObjectIndex] = null;
    }
}
