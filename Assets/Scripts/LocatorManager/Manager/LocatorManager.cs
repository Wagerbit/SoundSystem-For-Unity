using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace LocatorSystem
{
  public enum LocatorType
  {
    system,
    managers,
    floor,
    enemies,
    player,
    interactiveObject,
    cameras,
    audios,
    GUI
  }


  public class LocatorManager : Singleton<LocatorManager>
  {

    //[Inspect, Group("Managers", Priority = 1)]
    public GameObject system;
    //[Inspect, Group("Managers")]
    public GameObject managers;

    //[Inspect, Group("Scenary", Priority = 2)]
    public GameObject floor;

    //[Inspect, Group("Entities", Priority = 3)]
    public GameObject enemies;
    //[Inspect, Group("Entities")]
    public GameObject players;
    //[Inspect, Group("Entities")]
    public GameObject interactiveObjects;

    //[Inspect, Group("Cameras", Priority = 4)]
    public GameObject cameras;

    //[Inspect, Group("Audio", Priority = 5)]
    public GameObject audios;
    
    //[Inspect, Group("GUI", Priority = 6)]
    public GameObject GUI;
    //[Inspect, Group("GUI")]
    public EventSystem eventSystem;
    //[Inspect, Group("GUI")]


    public void RegisterAsLocator(LocatorType _type, GameObject _locator)
    {
      switch (_type)
      {
        case LocatorType.audios:
          audios = _locator;
          break;
        case LocatorType.cameras:
          cameras = _locator;
          break;
        case LocatorType.enemies:
          enemies = _locator;
          break;
        case LocatorType.floor:
          floor = _locator;
          break;
        case LocatorType.GUI:
          GUI = _locator;
          break;
        case LocatorType.interactiveObject:
          interactiveObjects = _locator;
          break;
        case LocatorType.managers:
          managers = _locator;
          break;
        case LocatorType.player:
          players = _locator;
          break;
        case LocatorType.system:
          system = _locator;
          break;
      }
    }
  }
}