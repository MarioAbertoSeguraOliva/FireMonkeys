using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animation))]
public class PlayAnimationWhenHit : MonoBehaviour {

    [SerializeField]private bool onlyOnce = true;
    [SerializeField]private AudioClip sound;
    [SerializeField]private float soundTimeOut;
    private bool played = false;

	void Start () {
	    
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision other) {
        if (onlyOnce && played)
            return;

        if (other.collider.CompareTag("Frisbee"))
        {
            GetComponent<Animation>().Play();
            if (sound)
                StartCoroutine(SoundRoutine());
            played = true;
       }
    }

    IEnumerator SoundRoutine()
    {
        yield return new WaitForSeconds(soundTimeOut);
        GetComponent<AudioSource>().PlayOneShot(sound);
    }

}
