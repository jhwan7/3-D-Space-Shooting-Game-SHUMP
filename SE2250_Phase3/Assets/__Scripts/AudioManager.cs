using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    public AudioMixerGroup master;
    public AudioMixerGroup effect;
    public AudioMixerGroup background;
    public AudioMixerGroup vfxEffect;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }
            

            DontDestroyOnLoad(gameObject);
        
        foreach(Sound s in sounds)
        {
           
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            if(s.tag == "VFX")
            {
                s.source.outputAudioMixerGroup = vfxEffect;
            }
            else if(s.tag == "BGM")
            {
                s.source.outputAudioMixerGroup = background;
            }
            else if(s.tag == "Effect")
            {
                s.source.outputAudioMixerGroup = effect;
            }
            else { s.source.outputAudioMixerGroup = master; }
            
            
            //s.source.dopplerLevel = 0f;
        }
    }
    private void Start()
    {
        Play("Background");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
