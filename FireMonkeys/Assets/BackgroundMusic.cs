using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {

	// Use this for initialization

    public static BackgroundMusic instance;
    
	void Start () {
        instance = this;
	}
	
	public void Play(AudioClip clip)
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

}
