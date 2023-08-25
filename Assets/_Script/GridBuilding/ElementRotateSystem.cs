using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementRotateSystem : MonoBehaviour
{
    public void MyInput(GameObject element, ElementsDatabase database)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateElement(element, database, true);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateElement(element, database, false);
            return;
        }
    }

    private void RotateElement(GameObject element, ElementsDatabase database,bool rotateRight)
    {
        Debug.Log(rotateRight);
    }
}
