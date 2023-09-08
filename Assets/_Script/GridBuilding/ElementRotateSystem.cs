using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class ElementRotateSystem : MonoBehaviour
{
    float rotateValue = 0;
    public float RotateElement(GameObject element,bool rotateRight)
    {
        float rotationAngle = rotateRight ? 90 : -90;
        rotateValue += rotationAngle;
        float returnValue = rotateValue;
        element.transform.GetChild(0).Rotate(0, rotateValue, 0);
        rotateValue = 0;
        return returnValue;
    }
}
