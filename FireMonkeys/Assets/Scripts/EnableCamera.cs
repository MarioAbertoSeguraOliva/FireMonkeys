using UnityEngine;
using System.Collections;

public class EnableCamera : MonoBehaviour {

    public Transform startPoint;
    public Transform endPoint;
    bool rendering;

	void Update () {
        //StartCoroutine(StartRendering());
	}
    
    //mario entra en los colliders de los lados tonto
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entering Collider1");
            Camera.main.enabled = false;
            this.GetComponent<Camera>().enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Exiting ColliderX");
            this.GetComponent<Camera>().enabled = false;
            Camera.main.enabled = true;
        }
    }
    //IEnumerator StartRendering()
   // {
    //    yield return 0;
    //}
}
