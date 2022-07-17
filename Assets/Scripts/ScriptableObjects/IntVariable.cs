using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "GMTKJAM/Runtime/Int Variable")]
public class IntVariable : ScriptableObject
{
    public event Action<int> onChange;

    private int current;

    public int Current
    {
        get => current;
        set
        {
            current = value;
            if (onChange != null) onChange(current);
        }
    }
}
