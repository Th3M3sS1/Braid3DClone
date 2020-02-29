using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Setting fields for target
    [SerializeField]
    Transform target;

    // Setting fields for camera
    private Vector3 cameraOffset;
    private float lookSensitivity = 2f;
    private float lookSmoothing = 2f;

    public bool lookAtPlayer = false;
    public bool rotateAroundPlayer = true;
    private bool isLocked = false;

    private Vector2 smoothedVelocity;
    private Vector2 currentLookingDirection;
    Quaternion turnAngle;

    // Awake is called before the Start is executed
    void Awake()
    {
        // Setting offset's value
        target = transform.root;
        cameraOffset = transform.position - target.position;        

        // Setting Cursor at the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // LateUpdate is called after the Update is executed
    void Update()
    {
        if (isLocked)
            return;

        // Setting the horizontal input of the mouse
        Vector2 rotateTargetX = new Vector2(Input.GetAxisRaw("Mouse X"), 0f);

        // Applying Scalleing to the vector
        Vector2 cameraRotateInput = Vector2.Scale(rotateTargetX, new Vector2(lookSensitivity * lookSmoothing, lookSensitivity * lookSmoothing));
        smoothedVelocity = Vector2.Lerp(smoothedVelocity, cameraRotateInput, 1 / lookSmoothing);

        currentLookingDirection += smoothedVelocity;

        // Setting rotation of the player for mouse horizontal movement
        target.localRotation = Quaternion.AngleAxis(currentLookingDirection.x, target.up);
        //target.rotation = Quaternion.Euler(0f, target.rotation.y, 0f);

        // Setting the rotation of camera around Player as mouse horizontal input
        if (rotateAroundPlayer)
        {
            Quaternion angle = Quaternion.FromToRotation(transform.forward, target.forward);
            turnAngle = Quaternion.AngleAxis(angle.y, Vector3.up);

            //Quaternion newAngle = Quaternion.Euler(turnAngle.x, 0f, turnAngle.z);

            cameraOffset = turnAngle * cameraOffset;
        }

        // Following the player
        Vector3 newPos = target.position + cameraOffset;
        //Vector3 newBetterPos = new Vector3(0, newPos.y, newPos.z);

        transform.position = Vector3.Slerp(transform.position, newPos, 0f);

        // Rotating camera around Player
        if (lookAtPlayer || rotateAroundPlayer)
            transform.LookAt(target);
    }

    // Function to lock player's and camera's horizontal rotaion 
    public void Lock(bool shouldLock)
    {
        isLocked = shouldLock;

        if (!shouldLock)
        {
            //Debug.Log(transform.rotation);
            //Debug.Log(transform.localEulerAngles.y);

            smoothedVelocity = new Vector2();

            currentLookingDirection = new Vector2(target.localEulerAngles.y, 0f);
        }
    }
}
