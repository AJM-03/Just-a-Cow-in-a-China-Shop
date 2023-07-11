using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    [SerializeField] private GameObject ragdollRoot;
    [SerializeField] private Animator anim;
    [SerializeField] private CowController cowController;
    [SerializeField] private Rigidbody mainRigidbody;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRagdolling = !isRagdolling;

            if (isRagdolling)
                EnableRagdoll();
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

        anim.SetBool("Ragdolling", false);
        anim.enabled = true;
        cowController.enabled = true;
        mainRigidbody.isKinematic = false;
    }

    private void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        anim.SetBool("Ragdolling", true);
        anim.enabled = false;
        cowController.enabled = false;
        mainRigidbody.isKinematic = true;
    }
}
