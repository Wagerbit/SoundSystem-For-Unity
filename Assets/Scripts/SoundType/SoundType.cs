using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
  public abstract class SoundType : MonoBehaviour
  {
    public abstract Type EnumType { get; }
    public abstract AudioMixer AudioMixerReference { get; set; }
    public abstract string AudioMixerGroup { get; }
    public abstract AudioClip GetAudioClip(Enum soundType);

  }
}

