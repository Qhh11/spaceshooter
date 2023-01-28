using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
   private static readonly string FirstPlay = "FirstPlay";
   private static readonly string MusicPref = "MusicPref";
   private static readonly string SFXPref = "SFXpref";
   [SerializeField] private Slider MusicvolumeSlider;
   [SerializeField] private Slider SFXvolumeSlider;
   private int firstPlayInt;
   [HideInInspector] public float musicVolume, SFXVolume;
   public AudioSource[] bgMusic;
   public AudioSource[] SFXs;
   private void Start()
   {
      firstPlayInt = PlayerPrefs.GetInt(FirstPlay);
      if (firstPlayInt == 0)
      {
         musicVolume = 1f;
         SFXVolume = 1f;
         MusicvolumeSlider.value = musicVolume;
         SFXvolumeSlider.value = SFXVolume;
         PlayerPrefs.SetFloat(MusicPref, musicVolume);
         PlayerPrefs.SetFloat(SFXPref, SFXVolume);
         PlayerPrefs.SetInt(FirstPlay, -1);
      }
      else
      {
         musicVolume = PlayerPrefs.GetFloat(MusicPref);
         MusicvolumeSlider.value = musicVolume;
         SFXVolume = PlayerPrefs.GetFloat(SFXPref);
         SFXvolumeSlider.value = SFXVolume;
        
      }
   }

   public void SaveSoundSettiings()
   {
      PlayerPrefs.SetFloat(MusicPref, MusicvolumeSlider.value);
      PlayerPrefs.SetFloat(SFXPref, SFXvolumeSlider.value);
   }

   private void OnApplicationFocus(bool hasFocus)
   {
      if (!hasFocus)
      {
         SaveSoundSettiings();
      }
   }

   public void UPdateSound()
   {
      for (int i = 0; i < bgMusic.Length; i++)
      {
         bgMusic[i].volume = MusicvolumeSlider.value;
      }
      
      for (int i = 0; i < SFXs.Length; i++)
      {
         SFXs[i].volume = SFXvolumeSlider.value;
      }
   }


  }
