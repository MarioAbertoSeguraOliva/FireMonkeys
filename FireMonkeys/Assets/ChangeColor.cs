using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeColor : MonoBehaviour {

    public AudioClip passedClip;

    void Awake()
    {
        GetComponent<AudioSource>().clip = passedClip;
    }
    void OnMouseEnter()
    {
        
    }

}
