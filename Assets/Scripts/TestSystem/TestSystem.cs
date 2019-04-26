using SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSystem : MonoBehaviour
{
  public GameObject fatherTest;

  public Text generalSoundText;
  public Text uISoundText;

  public VolumeSlider generalVolumeSlider;
  public VolumeSlider uIVolumeSlider;


  void Start()
  {
    Debug.Log("<color=red>#TestSystem#</color> Click here to see all the controls!\n\n" +
      "Use <color=Green>'Q'</color> to play a Main Manu audio (It keeps playing when the scene changes)\n" +
      "Use <color=Green>'W'</color> to play a Main Manu audio looped (It keeps playing when the scene changes)\n" +
      "Use <color=Green>'E'</color> to play a Main Manu audio (It <color=red>DON'T</color> keep playing when the scene changes)\n" +
      "Use <color=Green>'R'</color> to play a Main Manu audio looped (It <color=red>DON'T</color> keep playing when the scene changes)\n" +
      "Use <color=Green>'T'</color> to play a Main Manu audio looped with 0.5 of volume and testCube like father(It keeps playing when the scene changes)\n" +
      "Use <color=Green>'S'</color> To stop all audios\n" +
      "Use <color=Green>'C'</color> To change to SampleScene2\n");
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Q))
    {
      SoundSystemBankManager.Instance.PlayAudio(GeneralSoundType.MainMenu, false);
    }
    else if (Input.GetKeyDown(KeyCode.W))
    {
      SoundSystemBankManager.Instance.PlayAudio(GeneralSoundType.MainMenu, false, true);
    }
    else if (Input.GetKeyDown(KeyCode.E))
    {
      SoundSystemBankManager.Instance.PlayAudio(GeneralSoundType.MainMenu, true);
    }
    else if (Input.GetKeyDown(KeyCode.R))
    {
      SoundSystemBankManager.Instance.PlayAudio(GeneralSoundType.MainMenu, true, true);
    }
    else if (Input.GetKeyDown(KeyCode.T))
    {
      SoundSystemBankManager.Instance.PlaySoundIsolated(GeneralSoundType.MainMenu, null, 0.5f, null, null, null, fatherTest, true);
    }
    else if (Input.GetKeyDown(KeyCode.S))
    {
      SoundSystemBankManager.Instance.StopAudios(false);
    }
    else if (Input.GetKeyDown(KeyCode.C))
    {
      SceneManager.LoadScene(1);
    }
  }

  public void OnGeneralSliderChange()
  {
    generalSoundText.text = "General Volume: " + generalVolumeSlider.value + "dB";
  }
  public void OnUISliderChange()
  {
    uISoundText.text = "UI Volume: " + uIVolumeSlider.value + "dB";
  }
}
