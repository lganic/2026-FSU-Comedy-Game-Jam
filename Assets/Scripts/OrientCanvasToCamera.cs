using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OrientCanvasToCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 myPosition = transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(myPosition - cameraPosition, Vector3.up);

        transform.rotation = targetRotation;
    }
}
