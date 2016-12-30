using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

	public void Play()
    {
        GetComponent<Animation>().Play();
    }
}
