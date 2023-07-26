using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    [SerializeField] private bool toggleRagdollWithKey;
    [SerializeField] private GameObject ragdollRoot;
    [SerializeField] private Animator animator;
    [SerializeField] private CowController cowController;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private Collider mainCollider;
    [SerializeField] private Transform bodyTransform;

    private Rigidbody[] ragdollRigidbodies;
    private Collider[] colliders;


    private bool isRagdolling;

    void Awake()
    {
        ragdollRigidbodies = ragdollRoot.GetComponentsInChildren<Rigidbody>();
        colliders = ragdollRoot.GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRagdolling = !isRagdolling;

            if (isRagdolling)
                EnableRagdoll(new Vector3(Random.Range(-10000, 10000), Random.Range(1000, 10000), Random.Range(-10000, 10000)));
            else
                DisableRagdoll();
        }
    }

    private void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        mainRigidbody.transform.position = bodyTransform.position;

        animator.SetBool("Ragdolling", false);
        animator.enabled = true;
        cowController.enabled = true;
        mainRigidbody.isKinematic = false;
        mainCollider.enabled = true;
    }

    private void EnableRagdoll(Vector3 forceDirection)
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {   // Enable limb rigidbodies
            rigidbody.isKinematic = false;
        }

        foreach (var collider in colliders)
        {   // Enable limb colliders
            collider.enabled = true;
        }

        // Launch in a random direction
        bodyTransform.GetComponent<Rigidbody>().AddForce(forceDirection, ForceMode.Impulse);

        animator.SetBool("Ragdolling", true);
        animator.enabled = false;
        cowController.enabled = false;
        mainRigidbody.isKinematic = true;
        mainCollider.enabled = false;
    }

    public void ToggleRagdoll(Vector3 forceDirection, float forceMagnitude, Vector3 hitPoint)
    {
        Rigidbody bodyRigidbody = bodyTransform.GetComponent<Rigidbody>();

        Rigidbody hitRigidbody = ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();
        CowLimbCollision limb = hitRigidbody.GetComponent<CowLimbCollision>();
        
        Debug.Log(Vector3.Distance(hitRigidbody.position, hitPoint) * 100 + " - " + limb + " - " + limb.hitDetectionRadius);

        if (limb == null || Vector3.Distance(hitRigidbody.position, hitPoint) * 100 >= limb.hitDetectionRadius)
        {
            hitRigidbody = bodyRigidbody;
            limb = hitRigidbody.GetComponent<CowLimbCollision>();
        }


        if (forceMagnitude >= limb.minRagdollCollisionForce || isRagdolling)
        {
            isRagdolling = true;
            EnableRagdoll(Vector3.zero);

            if (!isRagdolling)
                forceDirection.y = limb.yStandingLaunch * Mathf.Abs(forceMagnitude / 2000);
            else
                forceDirection.y = limb.yRagdollLaunch * Mathf.Abs(forceMagnitude / 2000);

            hitRigidbody.AddForceAtPosition(forceDirection, hitPoint, ForceMode.Impulse);
            if (hitRigidbody != bodyRigidbody)
                bodyRigidbody.AddForceAtPosition(forceDirection, hitPoint, ForceMode.Impulse);
        }
    }
}
