using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof (ClimbCharacter))]
[RequireComponent(typeof (ClimbController))]
public class ClimbCharacterUserControl : MonoBehaviour
{
    private ClimbCharacter m_Character;       // A reference to the ThirdPersonCharacter on the object
    private ClimbController climbController;   // A reference to the ClimbController on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private bool m_Jump;                      
    private bool m_Climb;
    private bool isChargingFrisbee = false;
    private FrisbeeThrower frisbeeThrower;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<ClimbCharacter>();
        climbController = GetComponentInChildren<ClimbController>();
        climbController.climbEvent += ClimbEvent;
        frisbeeThrower = GetComponentInChildren<FrisbeeThrower>();
    }

    private void ClimbEvent(bool canClimb)
    {
        m_Climb = canClimb;
    }

    private void Update()
    {
        if (!m_Jump)
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        Vector3 m_Move = calculateMove();

        ClimbCharacter.Action action = getAction();

        if (isChargingFrisbee && !Input.GetMouseButton(0))
        {
            isChargingFrisbee = false;
            action = ClimbCharacter.Action.throwFrisbee;
        }

        frisbeeThrower.ManageFrisbee(action);

        // pass all parameters to the character control script
        m_Character.Move(m_Move, action);
        m_Jump = false;
    }

    private ClimbCharacter.Action getAction()
    {
        bool crouch = Input.GetKey(KeyCode.C);
        bool climb = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && m_Climb;
        bool comeOff = Input.GetKeyDown(KeyCode.E) && m_Climb;
        //bool climb = CrossPlatformInputManager.GetAxis("Vertical") > 0 && m_Climb;

        bool chargeFrisbee = Input.GetMouseButtonDown(0);
        bool throwFrisbee = Input.GetMouseButtonUp(0);
        bool shotFrisbee = Input.GetMouseButtonDown(1);

        if (climb)
        {
            m_Character.climbFinalPosition = climbController.climbPos;
            return ClimbCharacter.Action.climb;     
        }
        else if (m_Jump)
             return ClimbCharacter.Action.jump;
        else if (comeOff)
            return  ClimbCharacter.Action.comeOff;
        else if(!isChargingFrisbee && shotFrisbee)
        {
            return ClimbCharacter.Action.throwFrisbeeForward;
        }
        else if (chargeFrisbee && frisbeeThrower.HaveFrisbee)
        {
            isChargingFrisbee = true;
            return ClimbCharacter.Action.chargeFrisbee;
        }
        else if (throwFrisbee && frisbeeThrower.HaveFrisbee)
        {
            frisbeeThrower.throwDirection = getThrowDirection();
            return ClimbCharacter.Action.throwFrisbee;
        }
        else if (crouch)
            return ClimbCharacter.Action.crouch;
        else
            return ClimbCharacter.Action.move;

    }

    private Vector3 getThrowDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return (hitInfo.point - transform.position).normalized;
        }
        else
            return ray.direction;
    }

    private Vector3 calculateMove()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        Vector3 move;

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v * Vector3.forward + h * Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftShift)) move *= 0.3f;

        return move;
    }
}
