using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0,1)]
    public float volume = 1f;
    [Range(0.1f, 3)]
    public float pitch = 1f;

    public float time;

    public GameObject parent;

    public bool isUsingMaxTime;
    public float maxTime;
    

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

