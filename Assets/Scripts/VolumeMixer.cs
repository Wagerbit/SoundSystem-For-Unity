using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{

  public class VolumeMixer : Singleton<VolumeMixer>
  {
    public AudioMixer masterMixer;
    public AudioMixer soundEffects;

    public void SetMusicVolume(float musicVolume)
    {
      masterMixer.SetFloat("MusicVolume", musicVolume);
    }

    public void SetFXVolume(float fxVolume)
    {
      masterMixer.SetFloat("FXVolume", fxVolume);
    }

    public float GetVolumenOf(string volumeName)
    {
      float value;
      bool result = masterMixer.GetFloat(volumeName, out value);
      return (result ? value : 0f);
    }

    public void SetFXElementVolume(string element, float volume)
    {
      soundEffects.SetFloat(element, volume);
    }

    public float GetFXElementVolume(string element)
    {
      float value;
      bool result = soundEffects.GetFloat(element, out value);
      return (result ? value : 0f);
    }

    
  }
}

