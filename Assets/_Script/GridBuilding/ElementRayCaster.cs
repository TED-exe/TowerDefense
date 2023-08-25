using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementRayCaster : MonoBehaviour
{
    [SerializeField] private Transform[] rayCaster;
    [SerializeField] private LayerMask layerToHit;
    public bool CheckRayCollision()
    {
        foreach (Transform t in rayCaster)
        {
            if (Physics.Raycast(t.position, Vector3.down, out var hit, 15f, layerToHit))
            {
                return false;
            }
        }
        return true;
    }
}
