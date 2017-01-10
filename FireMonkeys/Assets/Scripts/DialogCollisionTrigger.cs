using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class DialogCollisionTrigger : MonoBehaviour {

    public AudioClip dialogueClip;
    public string dialogueFile;
    public bool onlyOnce = true;
    private bool displayed = false;
    public string otherTag = "Player";
    public UnityEvent onEnd;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(otherTag) && !displayed)
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            displayed = true;
            if (onEnd != null)
                StartCoroutine(WaitForEnd());
                
        }
    }

    IEnumerator WaitForEnd()
    {
        DialogueManager.Instance.withDialog = true;
        yield return new WaitWhile(() => DialogueManager.Instance.withDialog == true);
        onEnd.Invoke();
    }

    void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }
}
