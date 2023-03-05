using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CowController : MonoBehaviour
{
    public float moveSpeed;

    private Vector3 move;

    private PlayerInput input;
    private InputAction movementInput;

    void Awake()
    {
        input = new PlayerInput();
    }

    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * move;
    }
}
