using UnityEngine;
using System.Collections;
using System;

public class FrisbeeThrower : MonoBehaviour {

    [SerializeField] private GameObject frisbeePrefab;
    [SerializeField] private Transform hand;
    [HideInInspector] public float force = 15f;
    [SerializeField] private float frisbeeDelay = 0.3f;
    [SerializeField] private Vector2 forceRange = new Vector2( 5f, 60f );
    [SerializeField] private Vector2 gravityRange = new Vector2( 0.0001f, 1f);
    [SerializeField] private float maxChargeTime = 4f;
    [SerializeField] private float objectiveDepth = 20;
    private float chargingFrisbeeStartTime;
    [SerializeField] private bool m_HaveFrisbee = true;
    public bool HaveFrisbee
    {
        get
        {
            return frisbee.activeInHierarchy;
        }

        set
        {
            frisbee.SetActive(value);
        }
    }


    [HideInInspector] public GameObject frisbee;
    private bool isCharging;
    [HideInInspector]public Vector3 throwDirection;
    private float gravity;

    // Use this for initialization
    void Start () {
        if (hand == null)
            hand = transform.FindChild("EthanRightHandThumb1").transform;
        frisbee = (GameObject)Instantiate(frisbeePrefab, hand.position, hand.rotation * frisbeePrefab.transform.rotation, hand);

        HaveFrisbee = m_HaveFrisbee;
    }


    private IEnumerator ThrowFrisbee()
    {
        yield return new WaitForSeconds(frisbeeDelay);
        throwFrisbee();
    }

    public void addNewObjective(Ray ray)
    {
        RaycastHit hit;
        Vector3 objectivePoint;
        if (Physics.Raycast(ray, out hit))
            objectivePoint = hit.point;
        else
            objectivePoint = ray.GetPoint(objectiveDepth);

        frisbee.GetComponent<FrisbeeController>().objectives.Add(objectivePoint);
    }

    public void ManageFrisbee(ClimbCharacter.Action action)
    {
        
        if (action == ClimbCharacter.Action.throwFrisbee)
        {
            StartCoroutine(ThrowFrisbee());
            float chargingTime = Time.time - chargingFrisbeeStartTime;
            force = Mathf.Lerp(forceRange[0], forceRange[1], chargingTime / maxChargeTime);
            gravity = Mathf.Lerp(gravityRange[0], gravityRange[1], chargingTime / maxChargeTime);
        }
        else if (action == ClimbCharacter.Action.chargeFrisbee && !isCharging)
        {
            chargingFrisbeeStartTime = Time.time;
            charge();
        }else if (action == ClimbCharacter.Action.throwFrisbeeForward && !isCharging)
        {
            throwDirection = transform.forward;
            force = Mathf.Lerp(forceRange[0], forceRange[1], maxChargeTime / 4);
            gravity = Mathf.Lerp(gravityRange[0], gravityRange[1], maxChargeTime / 4);
            charge();
            Invoke("throwFrisbee", frisbeeDelay * 2);
        }

    }

    private void returnFrisbee()
    {
        Rigidbody frisbeeRb = frisbee.GetComponent<Rigidbody>();
        frisbeeRb.velocity = Vector3.zero;
        frisbeeRb.isKinematic = true;

        frisbee.GetComponent<Animation>().Stop();

        frisbee.GetComponent<Collider>().enabled = false;

        frisbee.transform.parent = hand;
        frisbee.transform.localPosition = Vector3.zero;
        frisbee.transform.rotation = hand.rotation * frisbeePrefab.transform.rotation;
    }

    public void throwFrisbee()
    {
        
        frisbee.transform.rotation = frisbeePrefab.transform.rotation;

        Rigidbody frisbeeRb = frisbee.GetComponent<Rigidbody>();

        frisbeeRb.isKinematic = false;
        frisbeeRb.velocity = throwDirection * force;
        frisbeeRb.mass = gravity;
        frisbee.transform.parent = null;

        frisbee.GetComponent<Collider>().enabled = true;

        frisbee.GetComponent<Animation>().Play();

        isCharging = false;

        frisbee.GetComponent<FrisbeeController>().Throw();
        
    }

    public void charge()
    {       
        returnFrisbee();
        frisbee.transform.parent = null;
        isCharging = true;
        frisbee.transform.rotation = frisbeePrefab.transform.rotation;
        frisbee.transform.position = hand.position;
    }

    public void Update()
    {
        if(isCharging)
            frisbee.transform.position = hand.position + hand.rotation * Vector3.forward *0.2f;

    }

}
