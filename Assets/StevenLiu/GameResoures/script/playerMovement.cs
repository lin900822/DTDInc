using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 10.0f;
    public float rotationSpeed=30.0f;
    public float jumpSpeed = 5.0f;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0, 0, -moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-moveSpeed * Time.deltaTime,0,0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(0, jumpSpeed, 0);
        }
    }
}
