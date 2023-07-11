using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToggle : MonoBehaviour
{
    [SerializeField] private GameObject ragdollRoot;
    [SerializeField] private Animator anim;
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

        mainRigidbody.transform.position = bodyTransform.position;

        anim.SetBool("Ragdolling", false);
        anim.enabled = true;
        cowController.enabled = true;
        mainRigidbody.isKinematic = false;
        mainCollider.enabled = true;
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

        bodyTransform.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10000, 10000), Random.Range(1000, 10000), Random.Range(-10000, 10000)), ForceMode.Impulse);

        anim.SetBool("Ragdolling", true);
        anim.enabled = false;
        cowController.enabled = false;
        mainRigidbody.isKinematic = true;
        mainCollider.enabled = false;
    }
}
