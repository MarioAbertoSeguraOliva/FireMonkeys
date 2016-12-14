using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ClimbCharacter : MonoBehaviour
{
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_JumpPower = 12f;
	[Range(1f, 4f)][SerializeField] float m_GravityMultiplier = 2f;
	[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float climbDuration = 1.13f;
    [SerializeField] float moveInAirFactor = 0.05f;

    public enum Action { move, jump, climb, crouch, chargeFrisbee, throwFrisbee, throwFrisbeeForward, comeOff, die, punch, jumpWall, dash };

	Rigidbody m_Rigidbody;
	Animator m_Animator;
	bool m_IsGrounded;
	float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	Vector3 m_GroundNormal;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
	bool m_Crouching;
    bool isClimbing = false;
    bool grabTheLedge = false;
    bool isChargingFrisbee = false;
    Vector3 climbInitPosition;
    [HideInInspector] public Vector3 climbFinalPosition;
    private ClimbController climbController;

    void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;

        climbController = GetComponentInChildren<ClimbController>();
        climbController.climbEvent += OnClimb;
	}

    void OnClimb(bool canClimb)
    {
        grabTheLedge = canClimb;
        if(grabTheLedge && !m_IsGrounded)
        {
            m_Rigidbody.isKinematic = true;
            Vector3 pos = transform.position;
            climbFinalPosition = climbController.climbPos;
            pos.y = climbFinalPosition.y - 1.6f;
            transform.position = pos;
        }else
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.isKinematic = false;
        }
    }

    public void Move(Vector3 move, Action action)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        if (action == Action.climb && !isClimbing)
            startClimbing();

        if(action == Action.comeOff && !isClimbing) 
        {
            grabTheLedge = false;
            m_Rigidbody.isKinematic = false;
        }

        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        CheckThrowStatus(action);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, 1/*move.z*/);
        m_ForwardAmount = move.z;
        
        if(!isClimbing && !grabTheLedge) {

            ApplyExtraTurnRotation();

            if (action == Action.jumpWall)
                jumpAgainstWall();

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(action);
            }
            else
            {
                HandleAirborneMovement(move);
            }

        }

		ScaleCapsuleForCrouching(action);
		PreventStandingInLowHeadroom();

		// send input and other state parameters to the animator
		UpdateAnimator(move, action);

    }

    private void jumpAgainstWall()
    {
        Vector3 jumpDir = climbController.normalJump.normalized;
        jumpDir.y = 0.5f;
        Debug.Log("JUMP "+ jumpDir.ToString());
        //m_Rigidbody.velocity = jumpDir.normalized * m_JumpPower * 5;
        m_Rigidbody.AddForce( jumpDir.normalized * m_JumpPower * 5, ForceMode.VelocityChange);

        transform.forward = new Vector3(jumpDir.x, 0, jumpDir.z).normalized;
    }

    private void CheckThrowStatus(Action action)
    {
        if (action == Action.chargeFrisbee)
            isChargingFrisbee = true;
        else if(action == Action.throwFrisbee)
            isChargingFrisbee = false;
    }

    IEnumerator climbingRoutine()
    {
        yield return new WaitForSeconds(climbDuration);
        transform.position = climbFinalPosition;
        isClimbing = false;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = Vector3.zero;
    }

    private void startClimbing()
    {
        Vector3 pos = transform.position;
        pos.y = climbFinalPosition.y - 0.55f;
        transform.position = pos;
        isClimbing = true;
        m_Rigidbody.isKinematic = true;

        StartCoroutine(climbingRoutine());
    }



    void ScaleCapsuleForCrouching(Action action)
	{
		if (m_IsGrounded && action == Action.crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers & ~LayerMask.GetMask("Frisbee"), QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers & ~LayerMask.GetMask("Frisbee"), QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}


	void UpdateAnimator(Vector3 move, Action action)
	{
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
		m_Animator.SetBool("Crouch", m_Crouching);
		m_Animator.SetBool("OnGround", m_IsGrounded);
        m_Animator.SetBool("Climb", isClimbing);
        m_Animator.SetBool("GrabTheLedge", grabTheLedge);
        m_Animator.SetBool("Punch", action == Action.punch);
        m_Animator.SetBool("Dash", action == Action.dash && m_IsGrounded);

        if (action == Action.die)
            m_Animator.SetTrigger("Die");


        if (action == Action.throwFrisbeeForward)
        {
            m_Animator.SetBool("Throw", true);
        }
        else
        m_Animator.SetBool("Throw", isChargingFrisbee);



        if (!m_IsGrounded)
			m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
		

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
		float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
		if (m_IsGrounded)
			m_Animator.SetFloat("JumpLeg", jumpLeg);


		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (m_IsGrounded && move.magnitude > 0)
			m_Animator.speed = m_AnimSpeedMultiplier;
		else
			// don't use that while airborne
			m_Animator.speed = 1;
	}


	void HandleAirborneMovement(Vector3 move)
	{
        // apply extra gravity from multiplier:
        

        transform.localPosition += transform.rotation * (moveInAirFactor * move);
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
		m_Rigidbody.AddForce(extraGravityForce);
        
		m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
	}


	void HandleGroundedMovement(Action action)
	{
		// check whether conditions are right to allow a jump:
		if (action == Action.jump && action != Action.crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
		{
			// jump!
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
			m_GroundCheckDistance = 0.1f;
            
        }

	}

	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}


	public void OnAnimatorMove()
	{
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		if (m_IsGrounded && Time.deltaTime > 0)
		{
			Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

			// we preserve the existing y part of the current velocity.
			v.y = m_Rigidbody.velocity.y;
			m_Rigidbody.velocity = v;
		}
	}


	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.applyRootMotion = false;
		}


        if (m_IsGrounded && grabTheLedge)
        {
            m_Rigidbody.isKinematic = false;
            grabTheLedge = false;
        }
    }
}

