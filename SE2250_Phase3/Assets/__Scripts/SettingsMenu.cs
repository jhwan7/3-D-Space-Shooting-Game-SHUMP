using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;

    // Start is called before the first frame update
   public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("master", volume);
    }
    public void SetEffectVolume(float volume)
    {
        audioMixer.SetFloat("effect", volume);
    }
    public void SetVFXVolume(float volume)
    {
        audioMixer.SetFloat("vfxSound", volume);
    }
    public void SetBackgroundVolume(float volume)
    {
        audioMixer.SetFloat("background", volume);
    }
}
