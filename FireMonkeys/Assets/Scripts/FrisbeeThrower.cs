using UnityEngine;
using System.Collections;
using System;

public class FrisbeeThrower : MonoBehaviour {

    [SerializeField] private GameObject frisbeePrefab;
    [SerializeField] private Transform hand;
    [SerializeField] private float force = 15f;

    private GameObject frisbee;
    [HideInInspector]public Vector3 throwDirection;

    // Use this for initialization
    void Start () {
        if (hand == null)
            hand = transform.FindChild("EthanRightHandThumb1").transform;
        frisbee = (GameObject)Instantiate(frisbeePrefab, hand.position, hand.rotation * frisbeePrefab.transform.rotation, hand);
        
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
        if (frisbee.transform.parent == null)
            returnFrisbee();

        frisbee.transform.rotation = frisbeePrefab.transform.rotation;

        Rigidbody frisbeeRb = frisbee.GetComponent<Rigidbody>();

        frisbeeRb.isKinematic = false;
        frisbeeRb.velocity = throwDirection * force;
        frisbee.transform.parent = null;

        frisbee.GetComponent<Collider>().enabled = true;

        frisbee.GetComponent<Animation>().Play();

        
    }
}
