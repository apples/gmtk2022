using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GridPosition))]
[RequireComponent(typeof(Health))]
public class HitTarget : MonoBehaviour
{
    public enum Type
    {
        Enemy,
        Player,
    }

    public Type type;
}
