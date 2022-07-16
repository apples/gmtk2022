using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private const int chunk_pow = 4;
    private const int chunk_width = 1 << chunk_pow;

    public TileGridReference tileGridReference;

    public TileGridGenerator generator;

    private Dictionary<Vector2Int, TileChunk> chunks = new Dictionary<Vector2Int, TileChunk>();

    void OnEnable()
    {
        Debug.Assert(tileGridReference.Current == null);
        tileGridReference.Current = this;
    }

    void OnDisable()
    {
        Debug.Assert(tileGridReference.Current == this);
        tileGridReference.Current = null;
    }

    void Start()
    {
        generator.Generate(this);
    }

    public bool TryGetTileData(Vector2Int coord, out TileData tileData)
    {
        var (chunkCoord, index) = BreakCoords(coord);

        if (chunks.TryGetValue(chunkCoord, out var chunk))
        {
            tileData = chunk.tiles[index].tileData;
            return true;
        }

        tileData = null;
        return false;
    }

    public void SetTileData(Vector2Int coord, TileData tileData)
    {
        var (chunkCoord, index) = BreakCoords(coord);

        if (!chunks.TryGetValue(chunkCoord, out var chunk))
        {
            chunk = new TileChunk();
            chunks[chunkCoord] = chunk;
        }

        if (chunk.tiles[index].tileObject != null)
        {
            Destroy(chunk.tiles[index].tileObject);
        }

        chunk.tiles[index].tileData = tileData;

        if (tileData != null)
        {
            chunk.tiles[index].tileObject = Instantiate(tileData.prefab, this.transform);
            chunk.tiles[index].tileObject.transform.localPosition = new Vector3(coord.x, 0, coord.y);
        }
        else
        {
            chunk.tiles[index].tileObject = null;
        }
    }

    public GridPosition GetOccupant(Vector2Int coord)
    {
        var (chunkCoord, index) = BreakCoords(coord);

        if (chunks.TryGetValue(chunkCoord, out var chunk))
        {
            return chunk.tiles[index].occupant;
        }

        return null;
    }

    public void MoveOccupant(GridPosition gridPosition, Vector2Int fromCoord, Vector2Int toCoord)
    {
        var (fromChunkCoord, fromIndex) = BreakCoords(fromCoord);
        var (toChunkCoord, toIndex) = BreakCoords(toCoord);

        if (chunks.TryGetValue(fromChunkCoord, out var fromChunk))
        {
            if (fromChunk.tiles[fromIndex].occupant == gridPosition)
            {
                fromChunk.tiles[fromIndex].occupant = null;
            }
        }

        if (chunks.TryGetValue(toChunkCoord, out var toChunk))
        {
            if (toChunk.tiles[toIndex].occupant == null)
            {
                toChunk.tiles[toIndex].occupant = gridPosition;
            }
            else
            {
                Debug.LogError($"Multiple occupants at {toCoord}");
            }
        }
    }

    public void NewOccupant(GridPosition gridPosition)
    {
        var (chunkCoord, index) = BreakCoords(gridPosition.Position);

        if (!chunks.TryGetValue(chunkCoord, out var chunk))
        {
            chunk = new TileChunk();
        }

        if (chunk.tiles[index].occupant == null)
        {
            chunk.tiles[index].occupant = gridPosition;
        }
        else
        {
            Debug.LogError($"Multiple occupants at {gridPosition.Position}");
        }
    }

    public void RemoveOccupant(GridPosition gridPosition)
    {
        var (chunkCoord, index) = BreakCoords(gridPosition.Position);

        if (!chunks.TryGetValue(chunkCoord, out var chunk))
        {
            return;
        }

        if (chunk.tiles[index].occupant == gridPosition)
        {
            chunk.tiles[index].occupant = null;
        }
    }

    private (Vector2Int, int) BreakCoords(Vector2Int coord)
    {
        var chunkCoord = new Vector2Int(coord.x >> chunk_pow, coord.y >> chunk_pow);
        var index = (coord.x - (chunkCoord.x << chunk_pow)) + chunk_width * (coord.y - (chunkCoord.y << chunk_pow));

        return (chunkCoord, index);
    }

    private class TileChunk
    {
        public Tile[] tiles = new Tile[chunk_width * chunk_width];
    }

    public struct Tile
    {
        public TileData tileData;
        public GameObject tileObject;
        public GridPosition occupant;
    }
}
