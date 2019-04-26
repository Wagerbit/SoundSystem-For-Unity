using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
  private static T _mInstance;
  private static bool m_ShuttingDown = false;

  public static T Instance
  {
    get
    {
      if (m_ShuttingDown)
      {
        Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
            "' already destroyed. Returning null.");
        return null;
      }

      if (!_mInstance)
      {
        _mInstance = (T)FindObjectOfType(typeof(T));
        if (!_mInstance)
        {
          GameObject gO = new GameObject(typeof(T).Name + " (Singleton)", typeof(T));
          _mInstance = gO.GetComponent<T>();
          DontDestroyOnLoad(gO);
        }
        return _mInstance;
      }
      return _mInstance;
    }
  }

  private void OnApplicationQuit()
  {
    //m_ShuttingDown = true;
  }
}
