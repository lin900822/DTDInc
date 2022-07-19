using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementVer2 : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;



   
    private Rigidbody theRigidbody;
    private CapsuleCollider theCollider;
   

    // Start is called before the first frame update
    void Start()
    {
        theRigidbody = GetComponent<Rigidbody>();
        theCollider = GetComponent<CapsuleCollider>();
       
    }

    // Update is called once per frame
    void Update()
    {


        PlayerControl();


    }


    void PlayerControl()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Time.deltaTime * speed * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Time.deltaTime * speed * Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Time.deltaTime * speed * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Time.deltaTime * speed * Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            theRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
 
        }
    }

  
}
