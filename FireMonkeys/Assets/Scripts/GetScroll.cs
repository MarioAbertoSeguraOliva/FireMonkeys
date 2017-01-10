using UnityEngine;
using System.Collections;

public class GetScroll : MonoBehaviour {

    public string dialogueFile;
    public AudioClip dialogueClip;
    public GameObject nextScroll;
    public AudioClip backgroundClip;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            other.GetComponent<GameOverManager>().respawnPosition = transform.position;
            if(nextScroll != null) nextScroll.SetActive(true);
            if(backgroundClip != null)
                BackgroundMusic.instance.Play(backgroundClip);

            Destroy(this.gameObject);
        }
    }
}
