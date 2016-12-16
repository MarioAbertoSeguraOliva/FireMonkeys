using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ChangeColor : MonoBehaviour {

    public AudioClip passedClip;

    public void OnMouseEnter()
    {
        GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<AudioSource>().PlayOneShot(passedClip);
    }

}
