using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject leftWheel;
    private GameObject rightWheel;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        leftWheel = transform.Find("CC_ME_Wheel_FL").gameObject;
        rightWheel = transform.Find("CC_ME_Wheel_FR").gameObject;
        Debug.Log(leftWheel.transform.eulerAngles);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (rb.velocity.magnitude < 2)
            {
                rb.AddRelativeForce(Vector3.forward * 20, ForceMode.Force);
            }
            else
            {
                rb.AddRelativeForce(Vector3.forward * 20, ForceMode.Acceleration);
            }
            
        }
        if (Input.GetKey(KeyCode.A) && rb.velocity.magnitude>1)
        {
            transform.Rotate(-Vector3.up * Time.deltaTime * 50, Space.Self);
           
            leftWheel.transform.Rotate(new Vector3(0, Mathf.Clamp(leftWheel.transform.eulerAngles.y, -30,0), 0)*2);
            rightWheel.transform.Rotate(new Vector3(0, Mathf.Clamp(rightWheel.transform.eulerAngles.y, -30, 0), 0)*2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.Self);
            leftWheel.transform.Rotate(Vector3.Slerp(new Vector3(0, 0, 0), new Vector3(0, 30, 0), 1f)*2);
            rightWheel.transform.Rotate(Vector3.Slerp(new Vector3(0, 0, 0), new Vector3(0, 30, 0), 1f)*2);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * 5, Space.Self);
        }
    }
}
