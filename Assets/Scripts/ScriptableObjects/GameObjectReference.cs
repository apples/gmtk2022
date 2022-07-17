using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectReference", menuName = "GMTKJAM/Runtime/Game Object Reference")]
public class GameObjectReference : ScriptableObject
{
    public GameObject Current { get; set; }
}
