using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    float groundSpeed;
    public float walkSpeed;
    public float runSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airSpeed;
    public float airDrag;
    public bool canJump;

    public float playerHeight;
    public LayerMask ground;
    public bool isGrounded;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rig;

    private void Start() {
        rig = GetComponent<Rigidbody>();
        rig.freezeRotation = true;
    }

    private void Update() {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        rig.drag = isGrounded ? groundDrag : airDrag;

        HandleInput();
        LimitSpeed();
    }

    void FixedUpdate() {
        MovePlayer();
    }

    void HandleInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && canJump && isGrounded) {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded) groundSpeed = runSpeed;
        else groundSpeed = walkSpeed;
    }

    void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float speed = isGrounded ? groundSpeed : airSpeed;
        rig.AddForce(moveDirection * speed, ForceMode.Force);
    }

    void LimitSpeed() {
        if (!isGrounded) return;

        Vector3 flatVel = new Vector3(rig.velocity.x, 0f, rig.velocity.z);
        if (flatVel.magnitude > groundSpeed) {
            Vector3 limitedVel = flatVel.normalized * groundSpeed;
            rig.velocity = new Vector3(limitedVel.x, rig.velocity.y, limitedVel.z);
        }
    }

    void Jump() {
        rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
        rig.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    Vector3 wallDir;

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("wall")) {
            wallDir = collision.contacts[0].normal;
            if (Input.GetKeyDown(KeyCode.Space)) {
                WallJump();
            };
        }
    }
    void WallJump() {
        //rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
        rig.AddForce((transform.up + wallDir).normalized * jumpForce, ForceMode.Impulse);
    }

    void ResetJump() {
        canJump = true;
    }

    public void PushPlayer(Vector3 dir) {
        rig.AddForce(dir, ForceMode.Force);
    }
}