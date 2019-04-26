using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
  [CreateAssetMenu(menuName = "SoundSystem/Scriptable Objects/Audio/GeneralSoundBank")]
  public class GeneralSoundBankSO : ScriptableObject
  {
    public List<GeneralSoundForSO> generalSoundList;

#if UNITY_EDITOR
    //This method is for initialize the UISoundList with the enum values
    [InspectMethod]
    public void InitGeneralSoundList()
    {
      List<GeneralSoundForSO> listTemp = new List<GeneralSoundForSO>();

      for (int i = 0; i < Enum.GetNames(typeof(GeneralSoundType)).Length; i++)
      {
        GeneralSoundForSO uISound = generalSoundList.Find(x => (int)x.generalSoundType == i);
        listTemp.Add(uISound ?? new GeneralSoundForSO((GeneralSoundType)i));
      }

      generalSoundList = listTemp;
    }
#endif
  }

  [System.Serializable]
  public class GeneralSoundForSO
  {
    public GeneralSoundType generalSoundType;
    public AudioClip generalSound;

    public GeneralSoundForSO(GeneralSoundType _soundEffectType)
    {
      generalSoundType = _soundEffectType;
    }
  }

}



