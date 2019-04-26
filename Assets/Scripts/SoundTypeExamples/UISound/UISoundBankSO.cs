using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundSystem
{
  [CreateAssetMenu(menuName = "SoundSystem/Scriptable Objects/Audio/UISoundBankSO")]
  public class UISoundBankSO : ScriptableObject
  {
    public List<UISoundForSO> UISoundList;

#if UNITY_EDITOR
    //This method is for initialize the UISoundList with the enum values
    [InspectMethod]
    public void InitUISoundList()
    {
      List<UISoundForSO> listTemp = new List<UISoundForSO>();

      for (int i = 0; i < Enum.GetNames(typeof(UISoundType)).Length; i++)
      {
        UISoundForSO uISound = UISoundList.Find(x => (int)x.uISoundType == i);
        listTemp.Add(uISound ?? new UISoundForSO((UISoundType)i));
      }

      UISoundList = listTemp;
    }
#endif
  }


  [System.Serializable]
  public class UISoundForSO
  {
    [SerializeField]
    public UISoundType uISoundType;
    public AudioClip uISound;

    public UISoundForSO(UISoundType _soundEffectType)
    {
      uISoundType = _soundEffectType;
    }
  }

}



