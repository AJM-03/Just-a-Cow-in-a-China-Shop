using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowController : MonoBehaviour
{
    public float moveForce;  // The cow's acceleration
    public float maxSpeed;  // How fast the cow can move

    private Vector3 forceDirection;  // How far the cow will move this frame
    private Rigidbody rb;  // The rigidbody physics component in the cow
    public Camera playerCamera;

    private PlayerControlsActionAsset inputActionAsset;
    private InputAction movementInput;

    void Awake()  // Runs before Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();  // Creates a reference to the cow's rigidbody component
        inputActionAsset = new PlayerControlsActionAsset();
    }

    private void OnEnable()  // When the cow object is enabled
    {
        inputActionAsset.Player.Charge.started += OnCharge;
        movementInput = inputActionAsset.Player.Move;
        inputActionAsset.Player.Enable();
    }



    private void OnDisable()  // When the cow object is disabled
    {
        inputActionAsset.Player.Charge.started -= OnCharge;
        inputActionAsset.Player.Disable();
    }


    void FixedUpdate()  // Fixed update should be used for physics calculations
    {
        forceDirection += movementInput.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * moveForce;  // Get the input direction relative to the camera direction
        forceDirection += movementInput.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * moveForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);  // Move the cow
        forceDirection = Vector3.zero;  // Reset force direction to zero so it can be recalculated next frame


        if (rb.velocity.y < 0f)  // Is the cow falling
            rb.velocity += Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;  // Makes the cow fall faster the longer it falls;


        Vector3 horizontalVelocity = rb.velocity;  // Make a horizontal velocity variable
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)  // If the velocity is over maxSpeed squared
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }



    private void LookAt()
    {
        Vector3 direction = rb.velocity;  // Looks in the direction of it's velocity
        direction.y = 0;  // Can't rotate up and down

        if (movementInput.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)  // If the player is moving
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;  // Don't keep spinning when there is no input
    }



    private void OnCharge(InputAction.CallbackContext value)  // When the charge input is pressed
    {
        if(IsGrounded())  // Check if the cow is grounded
        {
            // Charge here
        }
    }


    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);  // Shoot a line downwards
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))  // If the line hits something
            return true;  // You are grounded
        else
            return false;
    }    
    
    
    
    private Vector3 GetCameraForward(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera camera)
    {
        Vector3 right = camera.transform.right;
        right.y = 0;
        return right.normalized;
    }
}
