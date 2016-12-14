using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    private AudioClip dialogueAudio;
    string[] fileLines;
    string displaySubtitle;
    string dialogueFile;
    float subtitleTime;
    int line = 0;

    void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void BeginDialogue(string dialogueFile, AudioClip passedClip)
    {
        if(dialogueFile == null)
        {
            fileLines = null;
            return;
        }

        if(passedClip != null) 
            playAudio(passedClip);
        this.dialogueFile = dialogueFile;
        playSubtitle();
        subtitleTime = Time.time - 2.5f;
    }

    void playAudio(AudioClip passedClip)
    {
        dialogueAudio = passedClip;
        GetComponent<AudioSource>().clip = passedClip;
        GetComponent<AudioSource>().Play();
    }

    //TextAsset.text shows all lines, for this example fileLines[0] will work
    void playSubtitle()
    {
        TextAsset temp = (TextAsset)Resources.Load("Dialogues/" + dialogueFile);
        fileLines = temp.text.Split('\n');
        line = 0;
        displaySubtitle = fileLines[0];
        GetComponentInChildren<Text>().text = displaySubtitle;
        
    }

    void Update()
    {
        if (fileLines != null && Time.time - subtitleTime > 3.5f && line < fileLines.Length)
        {
            GetComponentInChildren<Text>().text = fileLines[line++];
            subtitleTime = Time.time;
        }
    }
}
