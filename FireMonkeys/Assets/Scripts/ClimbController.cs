using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ClimbController : MonoBehaviour
{

    [SerializeField]
    GameObject entity;
    [SerializeField]
    float radiusColliderRatio = 2f;
    [Range(0f, 1f)]
    [SerializeField]
    float allowedFlatness = 0.65f;
    [SerializeField]
    float frontOffset = 0.3f;
    [SerializeField]
    float upOffsetWhenClimb = 0.2f;
    [SerializeField]
    float autoClimbSlopeHeight = 0.2f;

    private bool canClimb = false;
    private bool canJump = false;

    public Vector3 climbPos;
    public Vector3 normalJump;
    public delegate void ClimbEvent(bool canDoIt);
    public event ClimbEvent climbEvent;
    public event ClimbEvent climbSlopeEvent;
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

    void Start()
    {
        if (entity == null) entity = transform.parent.gameObject;
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
        if (CheckIfCanClimb() != canClimb)
        {
            canClimb = !canClimb;

            if (moveDirectly() && climbSlopeEvent != null) climbSlopeEvent.Invoke(canClimb);
            else if (climbEvent != null) climbEvent.Invoke(canClimb);

        }
    }

    private bool moveDirectly()
    {
        float bottom = GetComponent<BoxCollider>().bounds.min.y;
        return bottom + autoClimbSlopeHeight > climbPos.y - upOffsetWhenClimb;
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
        CapsuleCollider entityCollider = entity.GetComponent<CapsuleCollider>();
        return Physics.CapsuleCast(
                        climbPos,
                        climbPos + new Vector3(0, entityCollider.height, 0),
                        entityCollider.radius * radiusColliderRatio,
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

        return top + entity.transform.forward * frontOffset;
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


    void Update()
    {
#if UNITY_EDITOR
        // helper to visualise the slope autoclimb height
        Bounds colliderBounds = GetComponent<BoxCollider>().bounds;
        float autoClimbHeight = colliderBounds.min.y + autoClimbSlopeHeight;
        Vector3 point1 = colliderBounds.min;
        point1.y = autoClimbHeight;
        Vector3 point2 = new Vector3( colliderBounds.min.x, autoClimbHeight, colliderBounds.max.z );
        Debug.DrawLine(point1, point2, Color.green);
#endif

    }
}