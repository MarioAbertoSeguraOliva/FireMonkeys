using UnityEngine;
using System.Collections;

public class DialogCollisionTrigger : MonoBehaviour {

    public AudioClip dialogueClip;
    public string dialogueFile;
    public bool onlyOnce = true;
    private bool displayed = false;
    public string otherTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(otherTag) && !displayed)
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            displayed = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }
}
