using UnityEngine;
using System.Collections;
using System;

public class FrisbeeThrower : MonoBehaviour {

    [SerializeField] private GameObject frisbeePrefab;
    [SerializeField] private Transform hand;
    [HideInInspector] public float force = 15f;
    [SerializeField] private float frisbeeDelay = 0.3f;
    [SerializeField] private float maxForce = 60f;
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxChargeTime = 4f;
    [SerializeField] private float objectiveDepth = 20;
    private float chargingFrisbeeStartTime;


    [SerializeField] public GameObject frisbee;
    private bool isCharging;
    [HideInInspector]public Vector3 throwDirection;

    // Use this for initialization
    void Start () {
        if (hand == null)
            hand = transform.FindChild("EthanRightHandThumb1").transform;
        frisbee = (GameObject)Instantiate(frisbeePrefab, hand.position, hand.rotation * frisbeePrefab.transform.rotation, hand);
        
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
            force = Mathf.Lerp(minForce, maxForce, chargingTime / maxChargeTime);
        }
        else if (action == ClimbCharacter.Action.chargeFrisbee)
        {
            chargingFrisbeeStartTime = Time.time;
            charge();
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
