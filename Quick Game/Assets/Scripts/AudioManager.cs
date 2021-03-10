using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = s.parent.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.time = s.time;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    private void Update()
    {
        foreach (Sound s in sounds)
        {
            if (s.isUsingMaxTime)
            {
                if (s.source.time >= s.maxTime)
                {
                    s.source.Stop();
                }
            }
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not working");
            return;
        } 
        s.source.Play();
    }
}
