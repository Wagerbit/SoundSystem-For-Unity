using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
  public class AudioSourceBuilder
  {
    #region ENUMERATORS

    private Enum enumType;

    #endregion

    private float? pitch;
    private float? volume;
    private float? effectArea;
    private float? spatialBlend;
    private Transform initPosition;
    private GameObject father;
    private bool? isLoop;
    private bool? stopOnSceneChange;


    #region Setters And Getters

    #region ENUMERATORS SETTERS

    public AudioSourceBuilder SetEnumType(Enum enumType) { this.enumType = enumType; return this; }

    #endregion

    public AudioSourceBuilder SetPitch(float? pitch) { this.pitch = pitch; return this; }
    public AudioSourceBuilder SetVolume(float? volume) { this.volume = volume; return this; }
    public AudioSourceBuilder SetEffectArea(float? effectArea) { this.effectArea = effectArea; return this; }
    public AudioSourceBuilder SetSpatialBlend(float? spatialBlend) { this.spatialBlend = spatialBlend; return this; }
    public AudioSourceBuilder SetInitPosition(Transform initPosition) { this.initPosition = initPosition; return this; }
    public AudioSourceBuilder SetFather(GameObject father) { this.father = father; return this; }
    public AudioSourceBuilder SetIsLoop(bool? isLoop) { this.isLoop = isLoop; return this; }
    public AudioSourceBuilder SetStopOnSceneChange(bool? stopOnSceneChange) { this.stopOnSceneChange = stopOnSceneChange; return this; }

    #region ENUMERATORS GETTERS

    public Enum GetEnumType() { return enumType; }

    #endregion

    public float? GetPitch() { return pitch; }
    public float? GetVolume() { return volume; }
    public float? GetEffectArea() { return effectArea; }
    public float? GetSpatialBlend() { return spatialBlend; }
    public Transform GetInitPosition() { return initPosition; }
    public GameObject GetFather() { return father; }
    public bool? GetIsLoop() { return isLoop; }
    public bool? GetStopOnSceneChange() { return stopOnSceneChange; }

    #endregion


    #region Default Values

    private readonly float defaultPitch = 1f;
    private readonly float defaultVolume = 1f;
    private readonly float defaultEffectArea = 200f;
    private readonly float defaultSpatialBlend = 1f;
    private readonly bool defaultIsLoop = false;
    private readonly bool defaultStopOnSceneChange = false;

    #endregion


    #region Builders

    private AudioSource PrepareAudioSourceAndGO(GameObject _audioGO)
    {
      _audioGO.transform.position = (initPosition != null) ? initPosition.position : Vector3.zero;
      if (this.father != null) { _audioGO.transform.SetParent(this.father.transform); } else { _audioGO.transform.parent = null; }

      AudioSource audio;
      audio = _audioGO.GetComponent<AudioSource>() ?? _audioGO.AddComponent<AudioSource>();
      audio.pitch = (pitch != null) ? (float)pitch : defaultPitch;
      audio.volume = (volume != null) ? (float)volume : defaultVolume;
      audio.maxDistance = (effectArea != null) ? (float)effectArea : defaultEffectArea;
      audio.spatialBlend = (spatialBlend != null) ? (float)spatialBlend : defaultSpatialBlend;
      audio.loop = (isLoop != null) ? (bool)isLoop : defaultIsLoop;
      audio.rolloffMode = AudioRolloffMode.Linear;

      return audio;
    }

    public void BuildSoundEffect()
    {
      stopOnSceneChange = stopOnSceneChange ?? defaultStopOnSceneChange;
      TemporaryAudioSource temporaryAudio = SoundSystemBankManager.Instance.GetUnusedAudioGameObject()
        .SetStopOnSceneChange((bool)stopOnSceneChange)
        .SetGameObjectAttached(father);

      GameObject audioGO = temporaryAudio.GetAudioGO();
      audioGO.name = Enum.GetName(GetEnumType().GetType(), GetEnumType()) + "Audio";

      AudioSource audio = PrepareAudioSourceAndGO(audioGO);

      SoundSystemBankManager.Instance.PlayAudio(GetEnumType(), (bool)stopOnSceneChange, audio);
    }

    #endregion
  }
}



