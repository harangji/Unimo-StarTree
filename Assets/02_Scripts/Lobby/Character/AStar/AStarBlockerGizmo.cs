using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBlockerGizmo : MonoBehaviour
{
    [SerializeField] private string colliderType;
    // Start is called before the first frame update
    void Awake()
    {
        checkColliderType();
    }

    private void OnDrawGizmos()
    {
        drawColGizmo();
    }
    private void checkColliderType()
    {
        Collider col = GetComponent<Collider>();
        if (col is BoxCollider)
        {
            colliderType = "Box";
        }
        else if (col is SphereCollider)
        {
            colliderType = "Sphere";
        }
        else if (col is CapsuleCollider)
        {
            colliderType = "Capsule";
        }
    }
    private void drawColGizmo()
    {
        Gizmos.color = Color.magenta;
        switch (colliderType)
        {
            case "Box":
                Gizmos.DrawWireCube(transform.position, transform.lossyScale);
                break;
            case "Sphere":
                Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x / 2);
                break;
            case "Capsule":
                Vector3 upSpherePos = transform.position + (0.5f * transform.lossyScale.y - transform.lossyScale.x / 2f) * Vector3.up;
                Vector3 downSpherePos = transform.position - (0.5f * transform.lossyScale.y - transform.lossyScale.x / 2f) * Vector3.up;
                Gizmos.DrawLine(upSpherePos + transform.lossyScale.x / 2f * transform.forward, downSpherePos + transform.lossyScale.x / 2f * transform.forward);
                Gizmos.DrawLine(upSpherePos - transform.lossyScale.x / 2f * transform.forward, downSpherePos - transform.lossyScale.x / 2f * transform.forward);
                Gizmos.DrawLine(upSpherePos + transform.lossyScale.x / 2f * transform.right, downSpherePos + transform.lossyScale.x / 2f * transform.right);
                Gizmos.DrawLine(upSpherePos - transform.lossyScale.x / 2f * transform.right, downSpherePos - transform.lossyScale.x / 2f * transform.right);
                Gizmos.DrawWireSphere(upSpherePos, transform.lossyScale.x / 2f);
                Gizmos.DrawWireSphere(downSpherePos, transform.lossyScale.x / 2f);
                break;
            default:
                Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x);
                break;
        }
    }
}
