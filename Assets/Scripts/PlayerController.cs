using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Setting fields for movement
    [SerializeField]
    private float movementSpeed = 8f;
    [SerializeField]
    private float jumpForce = 10f;

    private Rigidbody rb;   

    // For checking ground collider
    private float distanceToFeet;

    // Awake is called before the Start is executed
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        distanceToFeet = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Setting movement of the player
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        moveInput *= movementSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveInput.x, 0f, moveInput.y));

        // Setting jump of the player
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    // Checking if player is grounded or not
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToFeet + 0.1f);
    }
}
