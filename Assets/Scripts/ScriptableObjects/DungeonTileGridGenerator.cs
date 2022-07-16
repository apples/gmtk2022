using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonTileGridGenerator", menuName = "GMTKJAM/Tile Grid Generators/Dungeon")]
public class DungeonTileGridGenerator : TileGridGenerator
{
    public int width;
    public int height;

    public TileDataSet tileDataSet;

    public override void Generate(TileGrid tileGrid)
    {
        for (int x = 0; x < width; ++x)
        {
            tileGrid.SetTileData(new Vector2Int(x, 0), tileDataSet.wall);
            tileGrid.SetTileData(new Vector2Int(x, height - 1), tileDataSet.wall);
        }

        for (int y = 0; y < height; ++y)
        {
            tileGrid.SetTileData(new Vector2Int(0, y), tileDataSet.wall);
            tileGrid.SetTileData(new Vector2Int(width - 1, y), tileDataSet.wall);
        }

        for (int x = 1; x < width - 1; ++x)
        for (int y = 1; y < height - 1; ++y)
        {
            tileGrid.SetTileData(new Vector2Int(x, y), tileDataSet.floor);
        }
    }
}
