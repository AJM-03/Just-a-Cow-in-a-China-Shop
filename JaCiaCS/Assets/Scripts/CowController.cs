using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowController : MonoBehaviour
{
    public float moveForce;  // The cow's acceleration
    public float maxSpeed;  // How fast the cow can move

    private Vector3 toMove;  // How far the cow will move this frame
    private Rigidbody rb;  // The rigidbody physics component in the cow

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
        forceDirection += movementInput.ReadValue<Vector2>().x * GetCameraRight(playerCamera);  // Get the input direction relative to the camera direction
        forceDirection += movementInput.ReadValue<Vector2>().y * GetCameraForward(playerCamera);
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
}
