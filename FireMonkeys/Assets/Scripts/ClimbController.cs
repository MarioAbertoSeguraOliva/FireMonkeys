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
    public Vector3 normalJump;
    public delegate void EventType(bool canDoIt);
    public event EventType climbEvent;
    public event EventType jumpEvent;

    private static int ignoreRaycastMask;
    private static int secondCheckMask;
    private static int enemyMask;

    private bool canJump = false;

    void Awake()
    {
        ignoreRaycastMask = ~LayerMask.GetMask("Ignore Raycast");
        enemyMask = LayerMask.GetMask("Enemy");
        secondCheckMask = ignoreRaycastMask & ~LayerMask.GetMask("Player") & ~enemyMask;
    }

    void OnTriggerStay(Collider other)
    {
        LayerMask otherMask = 1 << other.gameObject.layer;
        if (other.isTrigger || other.CompareTag("Player") || otherMask == enemyMask)
            return;

        bool nowCanClimb = CheckIfCanClimb(out climbPos);
        if (nowCanClimb != canClimb)
        {
            canClimb = nowCanClimb;
            if(climbEvent != null)
                climbEvent.Invoke(canClimb);
        }

        /*if (!nowCanClimb)
            Debug.Log("nop " + canJump );*/

        if(canJump && jumpEvent != null)
        {
            canJump = false;
            Vector3 rayStart = transform.position;
            rayStart.y = GetComponent<BoxCollider>().bounds.center.y;
            RaycastHit hit;

            if (Physics.Raycast(rayStart, transform.forward,
                out hit, frontOffset * 2.5f,
                secondCheckMask))
            {
                normalJump = hit.normal;
/*#if UNITY_EDITOR
                // helper to visualise the ground check ray in the scene view
                Debug.DrawLine(hit.point, hit.point + hit.normal);
                Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.4f, Color.cyan);
                Debug.Break();
#endif*/
                jumpEvent.Invoke(true);
            }
        }
    }

    private bool CheckIfCanClimb(out Vector3 climbPos)
    {
        climbPos = Vector3.zero;
        canJump = true;

        CapsuleCollider personCollider = person.GetComponent<CapsuleCollider>();
        Vector3 top = new Vector3(
            GetComponent<BoxCollider>().bounds.center.x,
            GetComponent<BoxCollider>().bounds.max.y,
            GetComponent<BoxCollider>().bounds.center.z
        ) + person.transform.forward * frontOffset;

        RaycastHit hit;

        bool haveASurfaceToClimb = Physics.Raycast(top, Vector3.down,
            out hit, GetComponent<BoxCollider>().bounds.size.y,
            ignoreRaycastMask);

        bool surfaceIsNotFlat = hit.normal.y < allowedFlatness;

        if (haveASurfaceToClimb)
            canJump = false;

        if (!haveASurfaceToClimb || surfaceIsNotFlat)
        {
            return false;
        }
            

        climbPos = hit.point;
        climbPos.y += upOffsetWhenClimb;

        Vector3 rayStart = transform.position;
        rayStart.y = climbPos.y;
        Vector3 ray = climbPos - rayStart;

        if(Physics.Raycast(rayStart, ray,
            out hit, ray.magnitude,
            secondCheckMask))
        {
            return false;
        }

        return !Physics.CapsuleCast(
                hit.point,
                hit.point + new Vector3(0, personCollider.height, 0),
                personCollider.radius * radiusColliderRatio,
                Vector3.up
            ); ;
        }


    void OnTriggerExit(Collider other)
    {
        canClimb = false;
        climbEvent.Invoke(false);
    }

}
