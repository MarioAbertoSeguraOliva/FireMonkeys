using UnityEngine;
using System.Collections;

public class GetFrsibee : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<FrisbeeThrower>().HaveFrisbee = true;
            Destroy(this.gameObject);
        }
    }
}
