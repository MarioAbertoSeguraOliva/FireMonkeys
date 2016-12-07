using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class DialogTriggerOnKeyMove : MonoBehaviour {

    public AudioClip dialogueClip;
    public string dialogueFile;
    public bool onlyOnce = true;
    private bool displayed = false;

    void Update()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        if ((h != 0 || v != 0) && !displayed)
        {
            DialogueManager.Instance.BeginDialogue(dialogueFile, dialogueClip);
            displayed = true;
        }
    }
}
