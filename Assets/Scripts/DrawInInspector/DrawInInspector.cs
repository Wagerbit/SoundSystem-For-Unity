using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(UnityEngine.Object), true)]
public class DrawInInspector : UnityEditor.Editor {

  public override void OnInspectorGUI()
  {

    DrawDefaultInspector();

    Type type = target.GetType();

    foreach(MethodInfo method in type.GetMethods())
    {
      object[] attribute = method.GetCustomAttributes(typeof(InspectMethod), true);
      
      if (attribute.Length > 0) 
      {
        if (GUILayout.Button(method.Name))
        {
          //UnityEngine.Object[] objects = new UnityEngine.Object[1];
          //objects[0] = target;
          method.Invoke(target, null);
        }
      }
    }
  }
}
#endif