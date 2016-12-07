using UnityEngine;
using System.Collections;

public class EnterInstance : MonoBehaviour {

    public Transform destiny;

	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponentInParent<Transform>().transform.position = destiny.position;
    }
}
