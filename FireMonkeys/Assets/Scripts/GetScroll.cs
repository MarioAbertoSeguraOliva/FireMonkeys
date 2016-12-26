using UnityEngine;
using System.Collections;

public class GetScroll : MonoBehaviour {

    public string dialogueFile;
    public AudioClip dialogueClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            //Add note
            Destroy(this.gameObject);
        }
    }
}
