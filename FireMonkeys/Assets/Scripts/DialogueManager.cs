using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    string[] fileLines;
    string displaySubtitle;
    string dialogueFile;
    public bool withDialog = false;

    internal void BeginDialogue(object dialogueFile, AudioClip dialogueClip)
    {
        throw new NotImplementedException();
    }

    float subtitleTime;
    int line = 0;
    public float timePerLine = 0.2f;

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
        withDialog = true;
    }

    void playAudio(AudioClip passedClip)
    {
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
        if (fileLines != null  && line < fileLines.Length && Time.time - subtitleTime > timePerLine * (float)fileLines[Mathf.Max(0,line-1)].Length)
        {
            GetComponentInChildren<Text>().text = fileLines[line++];
            subtitleTime = Time.time;
        }
        else if((fileLines == null || line >= fileLines.Length) && withDialog)
        {
            GetComponentInChildren<Text>().text = "";
            withDialog = false;
        }
    }
}
