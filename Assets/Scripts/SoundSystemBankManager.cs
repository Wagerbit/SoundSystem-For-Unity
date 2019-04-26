using LocatorSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace SoundSystem
{
  public class SoundSystemBankManager : Singleton<SoundSystemBankManager>
  {
    public AudioMixer audioMixerMaster;
    public GameObject unusedAudioGO;
    public List<SoundType> listSoundType;
    private List<TemporaryAudioSource> totalUnusedAudioList, unusedAudioList, usedAudioList;

    #region MonoBehaviour

    private void Awake()
    {
      DontDestroyOnLoad(this);

      SceneManager.sceneUnloaded += OnSceneWasUnloaded;

      if (listSoundType == null)
        listSoundType = new List<SoundType>();

      RefressListSoundType();
    }

    public void OnSceneWasUnloaded(Scene scene)
    {
      StopAudios(true);
    }


    void Start()
    {
      InitTemporaryAudioList();
    }

    #endregion


    #region Sound Isolated

    /// <summary>
    /// In this methods you can customize the Audio that you want and set children of a GameObjet, 
    /// the values nullables take the default values.
    /// If you set a father, use StopAudio method for avoid deleting lost unused gameobjects.
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="pitch"></param>
    /// <param name="volume"></param>
    /// <param name="effectArea"></param>
    /// <param name="spatialBlend"></param>
    /// <param name="initPosition"></param>
    /// <param name="father"></param>
    /// <param name="isLoop"></param>
    public void PlaySoundIsolated(Enum enumType, float? pitch, float? volume, float? effectArea,
      float? spatialBlend, Transform initPosition, GameObject father, bool? isLoop)
    {
      new AudioSourceBuilder().SetEnumType(enumType).
        SetPitch(pitch).
        SetVolume(volume).
        SetEffectArea(effectArea).
        SetSpatialBlend(spatialBlend).
        SetInitPosition(initPosition).
        SetFather(father).
        SetIsLoop(isLoop).
        BuildSoundEffect();
    }

    #endregion


    #region PlayAudio Methods

    public void PlayAudio(Enum enumType, bool stopOnSceneChange, AudioSource _targetAudioSource = null)
    {
      TemporaryAudioSource temporaryAudio = null;

      if (_targetAudioSource == null)
      {
        temporaryAudio = GetUnusedAudioGameObject();

        GameObject audioSourceGO = temporaryAudio.GetAudioGO();

        //This is for put a name in the gameobject for a easy search in the Unity hierarchy
        string gameObjectName = Enum.GetName(enumType.GetType(), enumType);

        audioSourceGO.name = gameObjectName + "Audio";
        audioSourceGO.transform.parent = null;

        if (audioSourceGO.GetComponent<AudioSource>() != null)
          _targetAudioSource = audioSourceGO.GetComponent<AudioSource>();
        else
          _targetAudioSource = audioSourceGO.AddComponent<AudioSource>();

        temporaryAudio.SetStopOnSceneChange(stopOnSceneChange);
      }

      SoundType soundType = listSoundType.Find(x => x.EnumType == enumType.GetType());
      _targetAudioSource.clip = soundType.GetAudioClip(enumType);

      //This line put the correct Output in the AudioSource for link this sound with te audio configuration.
      AudioMixerGroup[] mixersGroup = audioMixerMaster.FindMatchingGroups(soundType.AudioMixerGroup);
      if (mixersGroup.Length != 0)
        _targetAudioSource.outputAudioMixerGroup = mixersGroup[0];
      else
        Debug.LogError("AudioMixerGroup " + soundType.AudioMixerGroup + " don't exist inside of AudioMixer");

      _targetAudioSource.Play();

      if (_targetAudioSource.loop == false)
      {
        if (temporaryAudio != null)
          temporaryAudio.SetCoroutineForStop(StartCoroutine(StopAudioWhenFinish(_targetAudioSource)));
        else
          StartCoroutine(StopAudioWhenFinish(_targetAudioSource));
      }
    }

    public void PlayAudio(Enum enunType, bool loop, bool stopOnSceneChange)
    {
      new AudioSourceBuilder().
        SetEnumType(enunType).
        SetIsLoop(true).
        SetStopOnSceneChange(stopOnSceneChange).
        BuildSoundEffect();
    }

    public void PlayAudioRandom(Enum[] soundsToPlay, bool stopOnSceneChange)
    {
      PlayAudio(soundsToPlay[UnityEngine.Random.Range(0, soundsToPlay.Length)], stopOnSceneChange);
    }

    #endregion


    #region UnusedAudio Methods

    /// <summary>
    /// Return and unusedAudio and refress the lists
    /// </summary>
    /// <returns></returns>
    public TemporaryAudioSource GetUnusedAudioGameObject()
    {
      TemporaryAudioSource audioToSend = unusedAudioList[0];

      usedAudioList.Add(unusedAudioList[0]);
      unusedAudioList.RemoveAt(0);
      audioToSend.GetAudioGO().SetActive(true);

      return audioToSend;
    }

    public void SetUnusedAudioGameObject(GameObject gameObject)
    {
      gameObject.transform.SetParent(unusedAudioGO.transform);
    }

    void UnattachAudioGO(GameObject audioGO)
    {
      audioGO.transform.SetParent(unusedAudioGO.transform);
      audioGO.transform.position = Vector3.zero;
      audioGO.name = "UnusedAudio";
      audioGO.SetActive(false);
    }

    void ResetAudioSourceValues(AudioSource _audio)
    {
      _audio.clip = null;
      _audio.pitch = 1f;
      _audio.volume = 1f;
      _audio.maxDistance = 500f;
      _audio.spatialBlend = 1f;
      _audio.loop = false;
      _audio.rolloffMode = AudioRolloffMode.Linear;
    }

    #endregion


    #region StopAudio Methods

    private IEnumerator StopAudioWhenFinish(AudioSource audio)
    {
      if (audio.clip != null) { yield return new WaitForSeconds(audio.clip.length); }

      audio.Stop();

      int index = -1;
      if ((index = usedAudioList.FindIndex(x => GameObject.ReferenceEquals(x.GetAudioGO(), audio.gameObject))) != -1)
      {
        UnattachAudioGO(audio.gameObject);

        unusedAudioList.Add(usedAudioList[index]);
        usedAudioList.RemoveAt(index);
      }

      ResetAudioSourceValues(audio);
    }

    public void StopAllAudiosOfAnGameObject(GameObject targetFather)
    {
      List<TemporaryAudioSource> elementsToRemove = new List<TemporaryAudioSource>();
      foreach (Transform child in targetFather.transform)
      {
        TemporaryAudioSource targetTemporaryAudioSource = usedAudioList.Find(x => x.GetAudioGO() == child.gameObject);
        if (targetTemporaryAudioSource != null)
        {
          AudioSource audio = targetTemporaryAudioSource.GetAudioGO().GetComponent<AudioSource>();
          audio.Stop();

          if (targetTemporaryAudioSource.GetCoroutineForStop() != null)
            StopCoroutine(targetTemporaryAudioSource.GetCoroutineForStop());

          unusedAudioList.Add(usedAudioList[usedAudioList.IndexOf(targetTemporaryAudioSource)]);
          elementsToRemove.Add(targetTemporaryAudioSource);

          ResetAudioSourceValues(audio);
        }
      }

      //Remove the elements added in elementsToRemove
      foreach (TemporaryAudioSource elementToRemove in elementsToRemove)
      {
        UnattachAudioGO(elementToRemove.GetAudioGO());
        usedAudioList.Remove(elementToRemove);
      }
    }

    public void StopAudios(bool isOnlyStopOnSceneChange)
    {
      List<TemporaryAudioSource> audiosToRemove = new List<TemporaryAudioSource>();
      foreach (var element in usedAudioList)
      {
        if (!isOnlyStopOnSceneChange || (element.GetStopOnSceneChange() && isOnlyStopOnSceneChange))
        {
          AudioSource audio = element.GetAudioGO().GetComponent<AudioSource>();
          audio.Stop();

          if (element.GetCoroutineForStop() != null)
            StopCoroutine(element.GetCoroutineForStop());

          unusedAudioList.Add(element);
          audiosToRemove.Add(element);
          UnattachAudioGO(audio.gameObject);
          ResetAudioSourceValues(audio);
        }

      }

      foreach (var element in audiosToRemove)
      {
        usedAudioList.Remove(element);
      }

    }

    #endregion


    #region Lists Methods

    /// <summary>
    /// Delete the positions on the listSountType with null value
    /// </summary>
    void RefressListSoundType()
    {
      List<SoundType> soundTypeTemp = new List<SoundType>();
      foreach (SoundType soundType in listSoundType)
      {
        if (soundType != null)
        {
          soundTypeTemp.Add(soundType);
        }
      }
      listSoundType = soundTypeTemp;
    }

    /// <summary>
    /// Init totalUnusedAudioList, unusedAudioList and usedAudioList and fill the first two.
    /// </summary>
    void InitTemporaryAudioList()
    {
      totalUnusedAudioList = new List<TemporaryAudioSource>();
      unusedAudioList = new List<TemporaryAudioSource>();
      usedAudioList = new List<TemporaryAudioSource>();

      TemporaryAudioSource tempAudio;

      foreach (Transform child in unusedAudioGO.transform)
      {
        tempAudio = new TemporaryAudioSource()
          .SetAudioGO(child.gameObject);
        ResetAudioSourceValues(child.gameObject.AddComponent<AudioSource>());
        totalUnusedAudioList.Add(tempAudio);
        unusedAudioList.Add(tempAudio);
      }
    }

    #endregion


    #region Volume Methods

    public void ChangeVolume(Enum soundType, float volume)
    {
      SoundType sound;
      sound = listSoundType.Find(x => x.ToString().Split(' ')[0] == soundType.ToString());

      if (sound != null)
      {
        AudioMixerGroup[] group = audioMixerMaster.FindMatchingGroups(sound.AudioMixerGroup);

        //The parameter name needs to be equal to audiomixer group.
        group[0].audioMixer.SetFloat(sound.AudioMixerGroup, volume);
      }
      else
      {
        Debug.Log("Sound not found");
      }
    }

    #endregion
  }
}


#region Classes

public class TemporaryAudioSource
{
  private GameObject gameObjectAttached;
  private GameObject audioGO;
  private Coroutine coroutineForStop;
  private bool stopOnSceneChange;


  #region Setters

  public TemporaryAudioSource SetGameObjectAttached(GameObject gameobject)
  {
    gameObjectAttached = gameobject;
    return this;
  }

  public TemporaryAudioSource SetAudioGO(GameObject gameobject)
  {
    audioGO = gameobject;
    return this;
  }

  public TemporaryAudioSource SetCoroutineForStop(Coroutine _coroutine)
  {
    coroutineForStop = _coroutine;
    return this;
  }

  public TemporaryAudioSource SetStopOnSceneChange(bool stopOnSceneChange)
  {
    this.stopOnSceneChange = stopOnSceneChange;
    return this;
  }

  #endregion

  #region Getters

  public GameObject GetGameObjectAttached()
  {
    return gameObjectAttached;
  }

  public GameObject GetAudioGO()
  {
    return audioGO;
  }

  public Coroutine GetCoroutineForStop()
  {
    return coroutineForStop;
  }

  public bool GetStopOnSceneChange()
  {
    return stopOnSceneChange;
  }

  #endregion

}

#endregion


