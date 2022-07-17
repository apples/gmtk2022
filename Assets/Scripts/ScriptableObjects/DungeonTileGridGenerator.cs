using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonTileGridGenerator", menuName = "GMTKJAM/Tile Grid Generators/Dungeon")]
public class DungeonTileGridGenerator : TileGridGenerator
{
    public int width;
    public int height;
    public int roomWidth;
    public int roomHeight;

    public TileDataSet tileDataSet;

    public GameObject exitPrefab;

    public float addPerRoomPerLevel = 1;

    public List<EnemySpawnConfig> spawns;

    public override void Generate(TileGrid tileGrid)
    {
        if (roomWidth < 4 || roomHeight < 4)
        {
            throw new System.InvalidOperationException("Room size too small!");
        }

        var rooms = new Room[width * height];

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                rooms[y * width + x] = new Room
                {
                    start = new Vector2Int(x * roomWidth, y * roomHeight),
                    index = y * width + x,
                    parent = y * width + x,
                };
            }
        }

        var walls = new List<Wall>((width - 1) * (height - 1) * 2 + width - 1 + height - 1);

        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                var room = rooms[y * width + x];

                if (y < height - 1)
                {
                    var nbor = rooms[(y + 1) * width + x];
                    var wall = new Wall { room1 = room, room2 = nbor, WE = false };
                    walls.Add(wall);
                    room.neighborN = nbor;
                    nbor.neighborS = room;
                    room.walls.Add(wall);
                    nbor.walls.Add(wall);
                }
                if (x < width - 1)
                {
                    var nbor = rooms[y * width + x + 1];
                    var wall = new Wall { room1 = room, room2 = nbor, WE = true };
                    walls.Add(wall);
                    room.neighborE = nbor;
                    nbor.neighborW = room;
                    room.walls.Add(wall);
                    nbor.walls.Add(wall);
                }
            }
        }

        Debug.Assert(walls.Count == (width - 1) * (height - 1) * 2 + width - 1 + height - 1);

        walls.Shuffle();

        foreach (var wall in walls)
        {
            var root1 = FindRoot(wall.room1.index);
            var root2 = FindRoot(wall.room2.index);

            if (root1 != root2)
            {
                Union(root1, root2);
                OpenWall(wall);
            }
        }

        foreach (var room in rooms)
        {
            var doorCount = room.walls.Count(x => x.open);

            if (doorCount == 1)
            {
                OpenWall(room.walls.Where(x => !x.open).OrderBy(x => x.room1 == room ? x.room2.rank : x.room1.rank).First());
            }
        }

        // GENERATE TILES

        foreach (var room in rooms)
        {
            for (var x = 0; x < roomWidth; ++x)
            {
                for (var y = 0; y < roomHeight; ++y)
                {
                    var coord = new Vector2Int(room.start.x + x, room.start.y + y);

                    if (x < roomWidth - 1 && y < roomHeight - 1)
                    {
                        tileGrid.SetTileData(coord, tileDataSet.floor);
                    }
                    else
                    {
                        tileGrid.SetTileData(coord, tileDataSet.wall);
                    }
                }
            }

            if (room.wallOpenN)
            {
                var coord = new Vector2Int(room.start.x + (roomWidth - 2) / 2, room.start.y + roomHeight - 1);
                tileGrid.SetTileData(coord, tileDataSet.floor);
                tileGrid.SetTileData(coord + new Vector2Int(1, 0), tileDataSet.floor);
                if (roomWidth % 2 == 0)
                {
                    tileGrid.SetTileData(coord + new Vector2Int(-1, 0), tileDataSet.floor);
                }
            }

            if (room.wallOpenE)
            {
                var coord = new Vector2Int(room.start.x + roomWidth - 1, room.start.y + (roomHeight - 2) / 2);
                tileGrid.SetTileData(coord, tileDataSet.floor);
                tileGrid.SetTileData(coord + new Vector2Int(0, 1), tileDataSet.floor);
                if (roomHeight % 2 == 0)
                {
                    tileGrid.SetTileData(coord + new Vector2Int(0, -1), tileDataSet.floor);
                }
            }

            if (room.start.x == 0)
            {
                for (var y = 0; y < roomHeight; ++y)
                {
                    var coord = new Vector2Int(room.start.x - 1, room.start.y + y);
                    tileGrid.SetTileData(coord, tileDataSet.wall);
                }
            }

            if (room.start.y == 0)
            {
                for (var x = 0; x < roomWidth; ++x)
                {
                    var coord = new Vector2Int(room.start.x + x, room.start.y - 1);
                    tileGrid.SetTileData(coord, tileDataSet.wall);
                }
            }

            if (room.start.x == 0 && room.start.y == 0)
            {
                var coord = new Vector2Int(room.start.x - 1, room.start.y - 1);
                tileGrid.SetTileData(coord, tileDataSet.wall);
            }
        }

        // spawns

        var playerSpawnRoom = rooms[Random.Range(0, rooms.Length)];
        var exitRoom = rooms[Random.Range(0, rooms.Length)];
        while (exitRoom == playerSpawnRoom)
        {
            exitRoom = rooms[Random.Range(0, rooms.Length)];
        }

        playerSpawnRoom.preventEnemy = true;
        exitRoom.preventEnemy = true;

        tileGrid.PlayerSpawnLocation = playerSpawnRoom.start + new Vector2Int(Random.Range(1, roomWidth - 2), Random.Range(1, roomHeight - 2));
        tileGrid.ExitLocation = exitRoom.start + new Vector2Int(Random.Range(1, roomWidth - 2), Random.Range(1, roomHeight - 2));

        tileGrid.SpawnThing(exitPrefab, tileGrid.ExitLocation);

        // enemies

        var difficulty = GameManager.Singleton.Level;

        float Weight(EnemySpawnConfig x) => x.startingDifficulty > difficulty ? 0f : (x.addWeightPerLevel * (difficulty - x.startingDifficulty));

        spawns.ForEach(x => x.weight = Weight(x));

        var totalWeight = spawns.Sum(x => x.weight);

        var enemyRooms = rooms.Where(x => !x.preventEnemy).ToList();

        var totalSpawns = addPerRoomPerLevel * difficulty * enemyRooms.Count;

        for (var i = 0; i < totalSpawns; ++i)
        {
            var room = enemyRooms[Random.Range(0, enemyRooms.Count)];

            var coord = room.start + new Vector2Int(Random.Range(0, roomWidth - 1), Random.Range(0, roomHeight - 1));

            if (tileGrid.GetOccupant(coord) != null)
            {
                break;
            }

            var roll = Random.Range(0f, totalWeight);

            int idx = 0;

            while (roll >= spawns[idx].weight && idx < spawns.Count - 1)
            {
                roll -= spawns[idx].weight;
                ++idx;
            }

            tileGrid.SpawnThing(spawns[idx].enemyPrefab, coord);
        }

        // disjoint sets

        int FindRoot(int i)
        {
            if (i != rooms[i].parent)
            {
                return FindRoot(rooms[i].parent);
            }
            return i;
        }

        void Union(int i, int j)
        {
            var iRoot = FindRoot(i);
            var jRoot = FindRoot(j);
            if (iRoot == jRoot) return;

            if (rooms[iRoot].rank > rooms[jRoot].rank)
            {
                rooms[jRoot].parent = iRoot;
            }
            else
            {
                rooms[iRoot].parent = jRoot;
                if (rooms[iRoot].rank == rooms[jRoot].rank)
                {
                    ++rooms[jRoot].rank;
                }
            }
        }

        void OpenWall(Wall wall)
        {
            wall.open = true;

            if (wall.WE)
            {
                wall.room1.wallOpenE = true;
            }
            else
            {
                wall.room1.wallOpenN = true;
            }
        }
    }

    private class Room
    {
        public Vector2Int start;
        public Room neighborN;
        public Room neighborS;
        public Room neighborE;
        public Room neighborW;
        public bool wallOpenE;
        public bool wallOpenN;
        public int rank = 0;
        public int index;
        public int parent;
        public List<Wall> walls = new List<Wall>(4);
        public bool preventEnemy;
    }

    private class Wall
    {
        public Room room1;
        public Room room2;
        public bool WE;
        public bool open;
    }

    [System.Serializable]
    public class EnemySpawnConfig
    {
        public GameObject enemyPrefab;
        public int startingDifficulty;
        public float addWeightPerLevel;

        [System.NonSerialized]
        public float weight;
    }
}
