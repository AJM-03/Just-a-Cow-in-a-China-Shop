using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowLimbCollision : MonoBehaviour
{
    public CowController cowController;

    private void OnCollisionEnter(Collision collision)
    {
        cowController.isGrounded = true;
    }
}
