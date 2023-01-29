using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSetting : MonoBehaviour
{
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SFXPref = "SFXpref";
    [HideInInspector]public float musicVolume, SFXVolume; 
    public AudioSource[] bgMusic;
    public AudioSource[] SFXs;
    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MusicPref);
        SFXVolume = PlayerPrefs.GetFloat(SFXPref);
        for (int i = 0; i < bgMusic.Length; i++)
        {
            bgMusic[i].volume = musicVolume;
        }
      
        for (int i = 0; i < SFXs.Length; i++)
        {
            SFXs[i].volume = SFXVolume;
        }
    }
}
