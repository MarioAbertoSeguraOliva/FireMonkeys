using UnityEngine;
using System.Collections;

public class TextDialogue : MonoBehaviour {
    public AudioClip dialogueClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DialogueManager.Instance.BeginDialogue(dialogueClip);
        }
    }
}
