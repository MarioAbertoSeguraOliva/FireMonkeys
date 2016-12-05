using UnityEngine;
using System.Collections;

public class DialogCollisionTrigger : MonoBehaviour {

    public AudioClip dialogueClip;
    public string dialogueFile;
    public bool onlyOnce = true;
    private bool displayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !displayed)
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            displayed = true;
        }
    }
}
