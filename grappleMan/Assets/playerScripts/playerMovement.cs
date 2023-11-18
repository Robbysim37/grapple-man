using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public Rigidbody rig;

    public float speed;
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = (transform.right * x) + (transform.forward * z);

        rig.velocity = movement * speed * Time.fixedDeltaTime;
    }
}
