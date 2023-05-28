using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;  // The same limb but in the fake cow that is used for animations
    private ConfigurableJoint cj;
    private Quaternion offset;
    public bool inverse;

    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
        offset = transform.localRotation;
    }

    void Update()
    {
        if (!inverse)
            cj.targetRotation = targetLimb.localRotation * offset;  // Sets the target rotation for the ragdoll
        else 
            cj.targetRotation = Quaternion.Inverse(targetLimb.localRotation) * offset;  
    }
}
