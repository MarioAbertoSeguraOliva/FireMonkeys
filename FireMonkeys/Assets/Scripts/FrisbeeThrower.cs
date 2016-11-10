using UnityEngine;
using System.Collections;
using System;

public class FrisbeeThrower : MonoBehaviour {

    [SerializeField] GameObject frisbeePrefab;
    [SerializeField] Transform hand;
    [SerializeField] float force = 15f;

    GameObject frisbee;

    // Use this for initialization
    void Start () {
        if (hand == null)
            hand = transform.FindChild("EthanRightHandThumb1").transform;
        frisbee = (GameObject)Instantiate(frisbeePrefab, hand.position, hand.rotation * frisbeePrefab.transform.rotation, hand);
        
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0))
        {
            if (frisbee.transform.parent != null)
                throwFrisbee();
            else
                returnFrisbee();
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

    private void throwFrisbee()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Rigidbody frisbeeRb = frisbee.GetComponent<Rigidbody>();

        frisbee.transform.rotation = frisbeePrefab.transform.rotation;

        frisbeeRb.isKinematic = false;
        frisbeeRb.velocity = ray.direction * force;
        frisbee.transform.parent = null;

        frisbee.GetComponent<Collider>().enabled = true;

        frisbee.GetComponent<Animation>().Play();
    }
}
