using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    public float movementSpeed = 5f;
    public float jumpForce = 8f;
    private Rigidbody rb;
    private bool isGrounded;

    //avoid repeat calling camera.main
    private Camera _mainCamera;

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkDespawn();
        Initialize(); //call this initializer when network is spawned
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    

    void Update()
    {
        if (!IsOwner || !Application.isFocused) return; //***for easier testing

        // Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * movementSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded (touching the ground)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Check if the player leaves the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

