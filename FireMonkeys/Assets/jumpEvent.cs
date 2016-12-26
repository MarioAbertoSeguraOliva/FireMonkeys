using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]
public class jumpEvent : MonoBehaviour {

    public float destroyTimeout = 4f;

    public void Jump(float force)
    {
        GetComponent<Animator>().SetBool("Jump", true);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(Vector3.up * force);
        Destroy(gameObject, destroyTimeout);
    }

}
