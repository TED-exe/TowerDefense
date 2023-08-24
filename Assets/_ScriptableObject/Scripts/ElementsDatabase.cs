using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElementsDatabase : ScriptableObject
{
    public List<ElementsInformation> element;
}
[Serializable]
public class ElementsInformation

{
    [field: SerializeField] public string elementName { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Vector2Int elementSize { get; private set; }
    [field: SerializeField] public GameObject elementPrefab { get; private set; }
    [field: SerializeField] public bool canPlaceOnThisElement { get; private set; }
    [field: SerializeField] public bool canByPlacedOnOtherElemets { get; private set; }
}
