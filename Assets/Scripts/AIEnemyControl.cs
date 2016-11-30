using System;
using UnityEngine;


[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (ClimbCharacter))]
public class AIEnemyControl : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ClimbCharacter character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for
    private ClimbController climbController;
    private bool canClimb = false;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ClimbCharacter>();
        climbController = GetComponentInChildren<ClimbController>();
        climbController.climbEvent += ClimbEvent;

        agent.updateRotation = false;
	    agent.updatePosition = true;
    }

    
    private Vector3 climbPos;

    private void ClimbEvent(bool climb)
    {
        canClimb = climb;
        climbPos = climbController.climbPos;
    }

    private void Update()
    {
        if (target != null)
            agent.SetDestination(target.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
            character.Move(Vector3.zero, ClimbCharacter.Action.move);
            //character.transform.position = new Vector3(2, 10, 3);
        else
            Move();
        Debug.DrawLine(climbPos, climbPos + Vector3.up * 3, Color.magenta);
    }

    private void Move()
    {
        if (canClimb)
        {
            canClimb = false;
            character.climbFinalPosition = climbPos;
            character.Move(Vector3.zero, ClimbCharacter.Action.climb);
        }
        else
            character.Move(agent.desiredVelocity, ClimbCharacter.Action.move);
            //character.Move(target.position - transform.position, ClimbCharacter.Action.move);

    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
