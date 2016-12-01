using System.Collections;
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

    protected enum state
    {
        Wander = 0,
        Chase = 1,
        ClimbInChase = 2,
        JumpInChase = 3
    }
    internal FSM fsm;

    private void Start()
    {
        // get the components on the object we need ( should be not null due to require component so no need to check )
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ClimbCharacter>();
        climbController = GetComponentInChildren<ClimbController>();
        climbController.climbEvent += ClimbEvent;
        GetComponentInChildren<PlayerDetector>().detectPlayerEvent += OnDetectPlayer;

        agent.updateRotation = false;
	    agent.updatePosition = true;

        fsm = new FSM(this, new FSM.StateMethod[] { Wander, Chase, ClimbInChase, JumpInChase });

        fsm.Start();
    }

    
    private void ClimbEvent(bool climb)
    {
        if(climb)
            fsm.ChangeState(state.ClimbInChase);
    }

    private void OnDetectPlayer(GameObject player)
    {
        if (player != null && target == null) 
            SetTarget(player.transform);
    }

    IEnumerator Wander()
    {
        while (target == null)
        {
            yield return 0;
        }
        fsm.ChangeState(state.Chase);
    }

    private bool ShouldJump()
    {
        Vector3 vector = target.position - transform.position;
        Vector3 difvector = agent.desiredVelocity.normalized + vector.normalized;
        return difvector.magnitude < 1 && vector.magnitude < 9;
    }

    IEnumerator Chase()
    {
        while (target != null && fsm.isState(state.Chase))
        {
            if (ShouldJump())
            {
                fsm.ChangeState(state.JumpInChase);
                break;
            }

            character.Move(agent.desiredVelocity, ClimbCharacter.Action.move);
            agent.SetDestination(target.position);
            yield return 0;
        }
        if(target == null)
            fsm.ChangeState(state.Wander);
    }

    IEnumerator ClimbInChase()
    {
        character.climbFinalPosition = climbController.climbPos;
        character.Move(Vector3.zero, ClimbCharacter.Action.climb);
        fsm.ChangeState(state.Chase);
        yield return 0;
    }

    IEnumerator JumpInChase()
    {
        Vector3 vector = target.position - transform.position;
        character.Move(vector, ClimbCharacter.Action.jump);
        agent.enabled = false;
        while (fsm.isState(state.JumpInChase))
        {
            yield return 0;
            vector = target.position - transform.position;
            character.Move(vector, ClimbCharacter.Action.move);
            
        }
        agent.enabled = true;
        fsm.ChangeState(state.Chase);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (fsm.isState(state.JumpInChase))
        {
            fsm.ChangeState(state.Chase);
            agent.enabled = true;
        }
    }

}
