using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileGridGenerator : ScriptableObject
{
    public abstract void Generate(TileGrid tileGrid);
}
