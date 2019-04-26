using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocatorSystem
{
  //[AdvancedInspector]
  public class Locator : MonoBehaviour
  {
    //[Inspect]
    public LocatorType locatorType;

    void Awake()
    {
      LocatorManager.Instance.RegisterAsLocator(locatorType, gameObject);
    }
  }
}


