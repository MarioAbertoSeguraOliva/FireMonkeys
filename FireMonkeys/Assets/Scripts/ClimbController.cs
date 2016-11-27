using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class ClimbController : MonoBehaviour {

    [SerializeField] GameObject person;
    [SerializeField] float radiusColliderRatio = 2f;
    [Range(0f, 1f)] [SerializeField] float allowedFlatness = 0.65f;
    [SerializeField] float frontOffset = 0.3f;
    [SerializeField] float upOffsetWhenClimb = 0.2f; //Remove errors

    private bool canClimb = false;

    public Vector3 climbPos;
    public delegate void ClimbEventType(bool canClimb);
    public event ClimbEventType climbEvent;

    public float y_Modifier = 0.5f;
    public float z_Modifier = 0.5f;
    public float jumpForce = 100.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        Gizmos.DrawRay(transform.position, direction);
    }
    void OnTriggerStay(Collider other)
    {
        bool nowCanClimb = CheckIfCanClimb(out climbPos);
        if (nowCanClimb != canClimb)
        {
            canClimb = nowCanClimb;
            climbEvent.Invoke(canClimb);
        }else
        {
            if (!nowCanClimb)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    Vector3 newJump = new Vector3(GetComponentInParent<Rigidbody>().transform.position.x ,
                                                  GetComponentInParent<Rigidbody>().transform.position.y * y_Modifier,
                                                  GetComponentInParent<Rigidbody>().transform.position.z * -z_Modifier);

                    GetComponentInParent<Rigidbody>().AddForce(Vector3.up * jumpForce);
                    GetComponentInParent<Rigidbody>().AddForce(Vector3.left * jumpForce);
                    GetComponentInParent<Rigidbody>().transform.Rotate(0, 180, 0);
                }
            }
        }
        

    }

    private bool CheckIfCanClimb(out Vector3 climbPos)
    {
        climbPos = Vector3.zero;

        CapsuleCollider personCollider = person.GetComponent<CapsuleCollider>();
        Vector3 top = new Vector3(
            GetComponent<BoxCollider>().bounds.center.x,
            GetComponent<BoxCollider>().bounds.max.y,
            GetComponent<BoxCollider>().bounds.center.z
        ) + person.transform.forward * frontOffset;

        RaycastHit hit;

        bool haveASurfaceToClimb = Physics.Raycast(top, Vector3.down,
            out hit, GetComponent<BoxCollider>().bounds.size.y,
            ~LayerMask.GetMask("Ignore Raycast"));

        bool surfaceIsNotFlat = hit.normal.y < allowedFlatness;

        if (!haveASurfaceToClimb || surfaceIsNotFlat)
            return false;

        climbPos = hit.point;
        climbPos.y += upOffsetWhenClimb;

        return !Physics.CapsuleCast(
                hit.point,
                hit.point + new Vector3(0, personCollider.height, 0),
                personCollider.radius * radiusColliderRatio,
                Vector3.up
            );
        }


    void OnTriggerExit(Collider other)
    {
        canClimb = false;
        climbEvent.Invoke(false);
    }

}
