using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRagdoll : MonoBehaviour
{
    [SerializeField] private float maxForce;
    [SerializeField] private float maxChargeTime;

    private float buttonDownTime;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buttonDownTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                RagdollToggle cow = hitInfo.collider.GetComponentInParent<RagdollToggle>();

                if (cow != null)
                {
                    float buttonDownDuration = Time.time - buttonDownTime;
                    float forcePercentage = buttonDownDuration / maxChargeTime;
                    float forceMagnitude = Mathf.Lerp(1, maxForce, forcePercentage);

                    Vector3 forceDirection = cow.transform.position - cam.transform.position;

                    forceDirection.y = 5;
                    forceDirection.Normalize();

                    Vector3 force = forceMagnitude * forceDirection;
                    cow.ToggleRagdoll(force, forceMagnitude, hitInfo.point);
                }
            }
        }
    }
}
