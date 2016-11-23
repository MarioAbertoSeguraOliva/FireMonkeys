using UnityEngine;
using System.Collections.Generic;
using System;

public class FrisbeeController : MonoBehaviour {
	
    [HideInInspector] public List<Vector3> objectives = new List<Vector3>();
    private bool flying = false;
    private Rigidbody physics;

    void Start()
    {
        physics = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void FixedUpdate () {
        if(flying)
            if(objectives.Count > 0 && Vector3.Magnitude( objectives[0] - transform.position ) < 0.5f)
            {
                objectives.RemoveAt(0);
                MoveToObjective();
            }
	}

    private void MoveToObjective()
    {
        if (objectives.Count > 0)
        {
            physics.velocity = Vector3.zero;
            physics.velocity = objectives[0] - transform.position;
            //physics.AddForce(objectives[0] - transform.position, ForceMode.);
        }
    }

    public void Throw() {
        flying = true;
        MoveToObjective();
    }
    


}
