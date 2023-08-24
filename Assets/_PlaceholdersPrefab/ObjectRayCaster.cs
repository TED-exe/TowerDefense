using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRayCaster : MonoBehaviour
{
    [SerializeField] private Transform[] rayCaster;
    [SerializeField] private LayerMask layerToHit;
    public bool CheckRayCollision()
    {
        foreach (Transform t in rayCaster)
        {
            Debug.Log(t.position);
            if (Physics.Raycast(t.position,Vector3.down,out var hit,15f,layerToHit))
            {
                Debug.Log(hit);
                return false;
            }
        }
        return true;
    }
}
