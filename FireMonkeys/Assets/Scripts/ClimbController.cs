using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class ClimbController : MonoBehaviour {

    [SerializeField] GameObject person;
    [SerializeField] float radiusColliderRatio = 2f;
    [Range(0f, 1f)] [SerializeField] float allowedFlatness = 0.65f;
    [SerializeField] float frontOffset = 0.3f;
    [SerializeField] float upOffsetWhenClimb = 0.2f;
    [SerializeField] float moveSlope = 0.2f;

    private bool canClimb = false;
    private bool canJump = false;

    public Vector3 climbPos;
    public Vector3 normalJump;
    public delegate void ClimbEvent(bool canDoIt);
    public event ClimbEvent climbEvent;
    public delegate void JumpEvent();
    public event JumpEvent jumpEvent;

    private static int ignoreRaycastMask;
    private static int secondCheckMask;
    private static int enemyMask;


    void Awake()
    {
        ignoreRaycastMask = ~LayerMask.GetMask("Ignore Raycast");
        enemyMask = LayerMask.GetMask("Enemy");
        secondCheckMask = ignoreRaycastMask & ~LayerMask.GetMask("Player") & ~enemyMask;
    }

    void OnTriggerStay(Collider other)
    {
        if (AvoidBadCases(other))
            return;

        DetectClimb();
        DetectDoubleJump();
    }

    private static bool AvoidBadCases(Collider other)
    {
        return other.isTrigger || other.CompareTag("Player") || (1 << other.gameObject.layer) == enemyMask;
    }


    private void DetectClimb()
    {
        if (climbEvent != null && CheckIfCanClimb() != canClimb)
        {
            canClimb = !canClimb;
            climbEvent.Invoke(canClimb);
        }
    }

    private void DetectDoubleJump()
    {
        RaycastHit hit;
        if (canJump && jumpEvent != null && TryToGetJumpHit(out hit))
        {
            normalJump = hit.normal;
            jumpEvent.Invoke();
            /*#if UNITY_EDITOR
                            // helper to visualise the ground check ray in the scene view
                            Debug.DrawLine(hit.point, hit.point + hit.normal);
                            Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.4f, Color.cyan);
                            Debug.Break();
            #endif*/
        }
    }

    private bool TryToGetJumpHit(out RaycastHit hit)
    {
        Vector3 rayStart = transform.position;
        rayStart.y = GetComponent<BoxCollider>().bounds.center.y;
        canJump = false;

        return Physics.Raycast(rayStart,
            transform.forward,
            out hit, frontOffset * 2.5f,
            secondCheckMask);
    }

    private bool CheckIfCanClimb()
    {
        RaycastHit hit;
        if (!CheckSurfaceTest(out hit)) return false;

        climbPos = hit.point; //Improve
        climbPos.y += upOffsetWhenClimb;

        if (!CheckColliderTest()) return false;

        return !EntityCanFitTest();
    }

    private bool EntityCanFitTest()
    {
        CapsuleCollider personCollider = person.GetComponent<CapsuleCollider>();
        return Physics.CapsuleCast(
                        climbPos,
                        climbPos + new Vector3(0, personCollider.height, 0),
                        personCollider.radius * radiusColliderRatio,
                        Vector3.up
                    );
    }

    bool CheckSurfaceTest(out RaycastHit hit)
    {
        Vector3 top = CalculateTopOfTheClimbPos();
        bool haveASurfaceToClimb = Physics.Raycast(top,
                                   Vector3.down, out hit,
                                   GetComponent<BoxCollider>().bounds.size.y,
                                   ignoreRaycastMask);
        bool surfaceIsFlat = hit.normal.y > allowedFlatness;

        canJump = !haveASurfaceToClimb;
        return haveASurfaceToClimb && surfaceIsFlat;
    }

    private Vector3 CalculateTopOfTheClimbPos()
    {
        Vector3 top = GetComponent<BoxCollider>().bounds.center;
        top.y = GetComponent<BoxCollider>().bounds.max.y;

        return top + person.transform.forward * frontOffset;
    }

    bool CheckColliderTest()
    {
        Vector3 rayStart = transform.position;
        rayStart.y = climbPos.y;
        Vector3 ray = climbPos - rayStart;

        return !Physics.Raycast(rayStart,
            ray,
            ray.magnitude,
            secondCheckMask);
    }


    void OnTriggerExit(Collider other)
    {
        canClimb = false;
        climbEvent.Invoke(false);
    }

}
