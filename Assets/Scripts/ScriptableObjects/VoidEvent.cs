using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoidEvent", menuName = "GMTKJAM/Runtime/Void Event")]
public class VoidEvent : ScriptableObject
{
    public event Action onTrigger;

    public void Trigger()
    {
        if (onTrigger != null) onTrigger();
    }
}
