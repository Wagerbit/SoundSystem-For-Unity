using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
  public enum UISoundType
  {
    OnClickOptionButton,
    OnClickPlayGameButton
  }

  public class UISound : SoundType
  {
    public override AudioMixer AudioMixerReference { get; set; }

    public override string AudioMixerGroup { get { return "UI"; } }

    public override Type EnumType
    {
      get { return typeof(UISoundType); }
    }

    public UISoundBankSO soundBank;

    public override AudioClip GetAudioClip(Enum soundType)
    {
      if (soundType.GetType() == EnumType)
      {
        AudioClip audio = soundBank.UISoundList.Find(x => x.uISoundType == (UISoundType)soundType).uISound;
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

