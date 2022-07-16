using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDataSet", menuName = "GMTKJAM/Tile Data Set")]
public class TileDataSet : ScriptableObject
{
    public TileData floor;
    public TileData wall;
}
