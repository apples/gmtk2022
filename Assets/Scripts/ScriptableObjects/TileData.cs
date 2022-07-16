using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "GMTKJAM/Tile Data")]
public class TileData : ScriptableObject
{
    public GameObject prefab;
    public TileType type;
}

public enum TileType
{
    Floor,
    Wall,
}
