using UnityEngine;
using System.Collections;

public class GetScroll : MonoBehaviour {

    public string dialogueFile;
    public AudioClip dialogueClip;
    public GameObject nextScroll;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            //Add note
            if(nextScroll != null) nextScroll.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
