using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animation))]
public class PlayAnimationWhenHit : MonoBehaviour {

    [SerializeField]private bool onlyOnce = true;
    private bool played = false;

	void Start () {
	    
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision other) {
        if (onlyOnce && played)
            return;

        Debug.Log("PUM!");
        if (other.collider.CompareTag("Frisbee"))
        {
            GetComponent<Animation>().Play();
            Debug.Log("Play...");
            played = true;
       }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<Animation>().Play();
            Debug.Log("Play...");

        }
    }
}
