using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    private AudioClip dialogueAudio;
    string[] fileLines;
    string displaySubtitle;
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

    public void BeginDialogue(AudioClip passedClip)
    {
        playAudio(passedClip);
        playSubtitle();
        subtitleTime = Time.time;
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
        TextAsset temp = (TextAsset)Resources.Load("Dialogues/" + dialogueAudio.name);
        fileLines = temp.text.Split('\n');
        displaySubtitle = fileLines[0];
        GetComponentInChildren<Text>().text = displaySubtitle;
        
    }

    void Update()
    {
        if (Time.time - subtitleTime > 2.5f){
            GetComponentInChildren<Text>().text = fileLines[line++];
            subtitleTime = Time.time;
        }
    }
}
