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

    void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        bool nowCanClimb = CheckIfCanClimb(out climbPos);
        if (nowCanClimb != canClimb)
        {
            canClimb = nowCanClimb;
            climbEvent.Invoke(canClimb);
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
