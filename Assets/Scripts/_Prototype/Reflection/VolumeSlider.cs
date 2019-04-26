using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SoundSystem
{
  [System.Serializable]
  public class VolumeSlider : Slider
  {
    public System.Enum soundType;
    public int enumSelected;

    new public void Awake()
    {
      InitSoundTypeEnum();

      if (EditorApplication.isPlaying)
      {
        Debug.Log("[VolumeSlider] Adding Listeners!");
        onValueChanged.AddListener(delegate { OnValueChange(); });
      }
    }

    new public void OnDestroy()
    {
      if (EditorApplication.isPlaying)
      {
        Debug.Log("[VolumeSlider] Removing Listeners!");
        onValueChanged.RemoveListener(delegate { OnValueChange(); });
      }
    }

    void OnValueChange()
    {
      SoundSystemBankManager.Instance.ChangeVolume(soundType, value);
    }

    public void InitSoundTypeEnum()
    {
      if(soundType == null)
      {
        List<string> soundList = new List<string>();

        foreach (SoundType sound in SoundSystemBankManager.Instance.listSoundType)
          soundList.Add(sound.name);

        if (soundList.Count == 0)
          soundList.Add("No SoundType in SoundSystemManager");

        soundType = CreateEnumFromArrays(soundList, enumSelected);
      }
    }

    /// <summary>
    /// Create new enum from arrays
    /// </summary>
    /// <param name="list">List with the values for the new enum</param>
    /// <param name="currentEnumValue">Value to set to the new enum</param>
    /// <returns></returns>
    public static System.Enum CreateEnumFromArrays(List<string> list, int currentEnumValue)
    {
      System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
      AssemblyName assemblyName = new AssemblyName("EnumSoundType");
      AssemblyBuilder assemblyBuilder = currentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.ReflectionOnly);
      ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
      EnumBuilder enumBuilder = moduleBuilder.DefineEnum("EnumSoundType", TypeAttributes.Public, typeof(int));

      for (int i = 0; i < list.Count; i++)
      {
        enumBuilder.DefineLiteral(list[i], i);
      }

      System.Type tempSoundTypeEnum = enumBuilder.CreateType();
      return (System.Enum)System.Enum.ToObject(tempSoundTypeEnum, currentEnumValue);
    }

    /// <summary>
    /// Method for create a custom slider
    /// </summary>
    [MenuItem("GameObject/UI/VolumeSlider")]
    public static void CreateVolumeSlider()
    {
      //Check if canvas exist in the scene
      Canvas canvas = FindObjectOfType<Canvas>();

      if(canvas == null)
      {
        GameObject newCanvas = new GameObject("Canvas");

        Canvas canvasComponent = newCanvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;

        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();

        canvas = canvasComponent;
      }

      //Creation of the Volume Slider GameObject
      GameObject volumeSliderGO = new GameObject("VolumeSlider");
      VolumeSlider volumeSlider = volumeSliderGO.AddComponent<VolumeSlider>();
      volumeSliderGO.transform.SetParent(canvas.transform);
      volumeSliderGO.transform.localPosition = Vector3.zero;

      RectTransform volumeSliderRectTransform = volumeSliderGO.GetComponent<RectTransform>();
      volumeSliderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160f);
      volumeSliderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 20f);

      //Creation of the slider GameObject components
      //Background
      GameObject background = new GameObject("Background");
      GameObjectUtility.SetParentAndAlign(background, volumeSliderGO);

      RectTransform backgroundRectTransform = background.AddComponent<RectTransform>();
      backgroundRectTransform.anchorMin = new Vector2(0f, 0.25f);
      backgroundRectTransform.anchorMax = new Vector2(1f, 0.75f);
      backgroundRectTransform.offsetMin = new Vector2(0f, 0f);
      backgroundRectTransform.offsetMax = new Vector2(0f, 0f);

      //Get the default background from unity
      Image backgroundImage = background.AddComponent<Image>();
      backgroundImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
      backgroundImage.type = Image.Type.Sliced;


      //Fill Area
      GameObject fillArea = new GameObject("Fill Area");
      GameObjectUtility.SetParentAndAlign(fillArea, volumeSliderGO);

      RectTransform fillAreaRectTransform = fillArea.AddComponent<RectTransform>();
      fillAreaRectTransform.anchorMin = new Vector2(0f, 0.25f);
      fillAreaRectTransform.anchorMax = new Vector2(1f, 0.75f);
      fillAreaRectTransform.anchoredPosition = new Vector2(-5f, 0f);
      fillAreaRectTransform.sizeDelta = new Vector2(-20f, 0f);


      //Fill
      GameObject fill = new GameObject("Fill");
      GameObjectUtility.SetParentAndAlign(fill, fillArea);

      RectTransform fillRectTransform = fill.AddComponent<RectTransform>();
      fillRectTransform.anchorMin = new Vector2(0f, 0f);
      fillRectTransform.anchorMax = new Vector2(0f, 1f);
      fillRectTransform.sizeDelta = new Vector2(10f, 0f);
      
      //Set the fillImage sprite with the default unity UISprite
      Image fillImage = fill.AddComponent<Image>();
      fillImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
      fillImage.type = Image.Type.Sliced;


      //Handle Slide Area
      GameObject handleSlideArea = new GameObject("Handle Slide Area");
      GameObjectUtility.SetParentAndAlign(handleSlideArea, volumeSliderGO);

      RectTransform handleSlideAreaRectTransform = handleSlideArea.AddComponent<RectTransform>();
      handleSlideAreaRectTransform.anchorMin = new Vector2(0f, 0f);
      handleSlideAreaRectTransform.anchorMax = new Vector2(1f, 1f);
      handleSlideAreaRectTransform.offsetMin = new Vector2(10f, 0f);
      handleSlideAreaRectTransform.offsetMax = new Vector2(-10f, 0f);


      //Handle
      GameObject handle = new GameObject("Handle");
      GameObjectUtility.SetParentAndAlign(handle, handleSlideArea);
      RectTransform handleRectTransform = handle.AddComponent<RectTransform>();

      handleRectTransform.anchorMin = new Vector2(0f, 0f);
      handleRectTransform.anchorMax = new Vector2(0f, 1f);
      handleRectTransform.sizeDelta = new Vector2(20f, 0f);

      //Setting the fillImage sprite with the default unity UISprite
      Image handleImage = handle.AddComponent<Image>();
      handleImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");


      //Prepare the VolumeSlider script
      volumeSlider.targetGraphic = handleImage;
      volumeSlider.fillRect = fillRectTransform;
      volumeSlider.handleRect = handleRectTransform;
      volumeSlider.minValue = -80f;
      volumeSlider.maxValue = 20f;
      volumeSlider.value = 0f;
    }
  }

  [CustomEditor(typeof(VolumeSlider))]
  [CanEditMultipleObjects]
  public class VolumeSliderEditor : Editor
  {
    VolumeSlider volumeSlider;
    SerializedProperty serializedProperty;

    public void Awake()
    {
      serializedProperty = serializedObject.FindProperty("enumSelected");
      volumeSlider = (VolumeSlider)target;
      volumeSlider.InitSoundTypeEnum();
    }

    public override void OnInspectorGUI()
    {
      EditorGUILayout.LabelField(new GUIContent("Sound Type"));
      volumeSlider.soundType = EditorGUILayout.EnumPopup(volumeSlider.soundType);

      Array enumValues = System.Enum.GetValues(volumeSlider.soundType.GetType());
      for (int i = 0; i < enumValues.Length; i++)
      {
        if (enumValues.GetValue(i).ToString() == volumeSlider.soundType.ToString())
        {
          serializedProperty.intValue = i;
          serializedObject.ApplyModifiedProperties();
        }
      }

      base.OnInspectorGUI();
    }
  }
}
