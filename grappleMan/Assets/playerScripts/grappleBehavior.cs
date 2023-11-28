using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleBehavior : MonoBehaviour
{
    public Camera cam;

    public GameObject ballPrefab;

    public float shootForce;

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Shoot();
        }
    }

    void Shoot() {
        Rigidbody ballRig = Instantiate(ballPrefab, cam.transform).GetComponent<Rigidbody>();
        ballRig.transform.SetParent(null);
        ballRig.AddForce(cam.transform.forward * shootForce);
    }
}
