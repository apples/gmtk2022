using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectReference", menuName = "GMTKJAM/Runtime/Game Object Reference")]
public class GameObjectReference : ScriptableObject
{
    public event Action<GameObject> onChange;

    private GameObject current;

    public GameObject Current
    {
        get => current;
        set
        {
            current = value;
            if (onChange != null) onChange(current);
        }
    }
}
