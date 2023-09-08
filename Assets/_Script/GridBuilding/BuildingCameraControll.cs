using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingCameraControll : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private Transform cameraHolder;
    private float verticalVelocity; //z;
    private float horizontalVelocity; //x;

    private void Update()
    {
        verticalVelocity = Input.GetAxis("Vertical") * cameraSpeed;
        horizontalVelocity = Input.GetAxis("Horizontal") * cameraSpeed;

        Vector3 moveDirection = new Vector3(horizontalVelocity, 0, verticalVelocity);

        cameraHolder.Translate(moveDirection * Time.deltaTime);
    }
}
