using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[AttributeUsage(AttributeTargets.Method)]
public class InspectMethod : Attribute {
}
#endif