using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileGridReference", menuName = "GMTKJAM/Runtime/Tile Grid Reference")]
public class TileGridReference : ScriptableObject
{
    public TileGrid Current { get; set; }
}
