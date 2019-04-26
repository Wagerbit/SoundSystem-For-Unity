using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
  public enum GeneralSoundType
  {
    MainMenu,
    Screen1,
    Screen2,
    Credits
  }

  public class GeneralSound : SoundType
  {
    public override AudioMixer AudioMixerReference { get; set; }

    public override string AudioMixerGroup { get { return "General"; } }

    public override Type EnumType
    {
      get { return typeof(GeneralSoundType); }
    }

    public GeneralSoundBankSO soundBank;

    public override AudioClip GetAudioClip(Enum soundType)
    {
      if (soundType.GetType() == EnumType)
      {
        AudioClip audio = soundBank.generalSoundList.Find(x => x.generalSoundType == (GeneralSoundType)soundType).generalSound;
        return audio ? audio : null;
      }
      else
      {
        Debug.LogError("Incorrect Type in GetAudioClip() method");
        return null;
      }
    }
  }
}

