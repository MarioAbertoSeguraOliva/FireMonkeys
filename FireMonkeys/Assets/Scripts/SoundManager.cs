using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    private AudioSource source;

    [System.Serializable]
    public struct State
    {
        public string name;
        public AudioClip[] sounds;
        public float timeBetweenSounds;
        public bool onlyOnce;
        public bool overlapWithHimself;
    }


    public State[] states;

    private const int noSound = -1;
    private int haveToPlay = noSound;
    private int lastPlayed = noSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        StartCoroutine(PlayCorutine());
    }

    public void Play(int id)
    {
        if (id == noSound) return;

        int playing = Playing();
        if (playing != id || (playing == id && states[id].overlapWithHimself))
            haveToPlay = id;
    }

    public void Play(string name)
    {
        Play(IdFromName(name));
    }

    public int IdFromName(string name)
    {
        for (int i = 0; i < states.Length; i++)
            if (states[i].name == name)
                return i;
        return noSound;
    }


    public void Stop()
    {
        haveToPlay = noSound;
        source.Stop();
    }

    private IEnumerator PlayCorutine()
    {

        while (true)
        {
            yield return new WaitWhile(() => haveToPlay == noSound);

            State actualState = states[haveToPlay];

            int id = UnityEngine.Random.Range(0, actualState.sounds.Length);
            source.PlayOneShot(actualState.sounds[id]);
            lastPlayed = haveToPlay;

            if (actualState.onlyOnce)
                haveToPlay = noSound;
            else
                yield return new WaitForSeconds(actualState.timeBetweenSounds);

        }

    }

    public int Playing()
    {
        if (source.isPlaying) return lastPlayed;
        return noSound;
    }

    internal bool isPlaying(string name)
    {
        return Playing() == IdFromName(name);
    }
}
