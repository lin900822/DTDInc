using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform cameraFollow;
    public Transform playerFollow;
    private float xInput;
    private float yInput;


    private void Update()
    {
        xInput += Input.GetAxis("Mouse X");
        yInput += Input.GetAxis("Mouse Y");

       playerFollow.localRotation = Quaternion.Euler(0, xInput, 0);
       cameraFollow.localRotation = Quaternion.Euler(yInput, 0, 0); 
    }


}
