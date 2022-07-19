using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [Header("Index")]
    public float speed=10f;
    public float rotateSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    private void movement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var verticalMove = transform.forward * speed * vertical*Time.deltaTime;
        
        transform.Rotate(Vector3.up, horizontal * rotateSpeed);

        characterController.Move(verticalMove);
        
    }
}
