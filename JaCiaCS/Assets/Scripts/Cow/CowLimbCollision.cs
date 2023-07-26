using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowLimbCollision : MonoBehaviour
{
    [SerializeField] private CowController cowController;
    public float minRagdollCollisionForce;
    public float yStandingLaunch;
    public float yRagdollLaunch;
    public float hitDetectionRadius;

    private void OnCollisionEnter(Collision collision)
    {
        cowController.isGrounded = true;
    }
}
