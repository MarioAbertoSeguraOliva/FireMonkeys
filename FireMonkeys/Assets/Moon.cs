using UnityEngine;
using System.Collections;

public class Moon : MonoBehaviour {

    [HideInInspector]
    public GameObject objective;
    private float radius;

	// Use this for initialization
	void Start () {
        radius = GetComponent<SphereCollider>().radius * transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update () {
	    if(objective != null)
        {
            Vector3 direction = objective.transform.position - transform.position;
            if(direction.magnitude < radius)
            {
                objective.GetComponent<Health>().Amount = 0;
                Destroy(gameObject, 2f);
            }
            transform.position += direction.normalized * Time.deltaTime * 14;
        }
	}


}
