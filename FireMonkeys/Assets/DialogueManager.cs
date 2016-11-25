﻿using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager Instance { get; private set; }
    private AudioClip dialogueAudio;
    private GUIStyle subtitleStyle = new GUIStyle();
    string[] fileLines;
    string displaySubtitle;

    void Awake()
    {
	    if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;
        TextAsset temp = (TextAsset) Resources.Load("Dialogues/" + dialogueAudio.name);
        //TextAsset temp = Resources.Load("Dialogues/"+dialogueAudio.name) as TextAsset;
        fileLines = temp.text.Split('\n');
        displaySubtitle = fileLines[0];
        Debug.Log(displaySubtitle);
    }

    public void BeginDialogue(AudioClip passedClip)
    {
        dialogueAudio = passedClip;
        GetComponent<AudioSource>().clip = passedClip;
        GetComponent<AudioSource>().Play();
    }

    void OnGUI()
    {
        if (dialogueAudio != null && GetComponent<AudioSource>().name == dialogueAudio.name)
        {
            GUI.depth = -1001;
            subtitleStyle.fixedWidth = Screen.width / 1.5f;
            subtitleStyle.wordWrap = true;
            subtitleStyle.alignment = TextAnchor.MiddleCenter;
            subtitleStyle.normal.textColor = Color.white;
            subtitleStyle.fontSize = Mathf.FloorToInt(Screen.height * 0.0225f);

            Vector2 size = subtitleStyle.CalcSize(new GUIContent());
            GUI.contentColor = Color.black;
            GUI.Label(new Rect(Screen.width / 2 - size.x / 2 + 1, Screen.height / 1.25f - size.y + 1, size.x, size.y), displaySubtitle, subtitleStyle);
            GUI.contentColor = Color.white;
            GUI.Label(new Rect(Screen.width/2 - size.x /2, Screen.height/1.25f - size.y, size.x, size.y), displaySubtitle, subtitleStyle);
        }
    }
}
