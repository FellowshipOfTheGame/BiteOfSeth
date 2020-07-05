using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSlicer : MonoBehaviour
{
    public Tilemap tilemapToSlice = null;
    public TilesetObjects tilesetObjects = null;
    List<Tilemap> workingTilemaps = null;
    List<GameObject> rooms = null;

    void Start()
    {
        if (tilemapToSlice == null || tilesetObjects == null)
        {
            Debug.LogError("Slicer with null arguments");
        }
        else
        {
            SliceTilemap(tilemapToSlice, tilesetObjects);
            foreach(GameObject t in rooms)
            {
                RoomBehavior r = t.gameObject.GetComponent<RoomBehavior>();
                if (r != null)
                {
                    r.SpawnRoom(tilesetObjects);
                }
            }
        }
    }

    void SliceTilemap(Tilemap tilemapToSlice, TilesetObjects tilesetObjects)
    {
        workingTilemaps = new List<Tilemap>();

        tilemapToSlice.gameObject.SetActive(false);

        Tilemap wallMap = Instantiate(tilemapToSlice, transform);
        wallMap.gameObject.SetActive(true);
        wallMap.gameObject.name = "walls";

        // turn checkpoints to walls then clear non-walls
        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = wallMap.GetTile(localPlace);
            if (tile != tilesetObjects.wallTile && tile != tilesetObjects.checkpointTile && tile != tilesetObjects.fakeWallTile)
            {
                wallMap.SetTile(localPlace, null);
            }
        }

        // create tilemaps for each room with FloodFill
        rooms = new List<GameObject>();
        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (true)
            {
                if (tileInMaps(workingTilemaps, localPlace) == false)
                {
                    GameObject newRoom = new GameObject("room " + workingTilemaps.Count.ToString());
                    newRoom.transform.parent = transform;
                    newRoom.AddComponent<RoomBehavior>();
                    Tilemap newRoomTilemap = Instantiate(wallMap, newRoom.transform);
                    newRoomTilemap.FloodFill(localPlace, tilesetObjects.wallTile);
                    newRoomTilemap.gameObject.SetActive(true);
                    newRoomTilemap.gameObject.name = "base tilemap";
                    workingTilemaps.Add(newRoomTilemap);
                    rooms.Add(newRoom);
                }
            }
        }

        Tilemap startingTilemap = null;

        // clear wallmap from rooms
        foreach (Tilemap tilemap in workingTilemaps)
        {
            // remove wall map to get only fill
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (wallMap.HasTile(localPlace))
                {
                    tilemap.SetTile(localPlace, null);
                }
            }

            Tilemap readdTilemap = Instantiate(tilemap);
            // readd the borders
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (wallMap.HasTile(localPlace) && HasNeighbor(tilemap, localPlace))
                {
                    readdTilemap.SetTile(localPlace, wallMap.GetTile(localPlace));
                }
            }
            foreach (var pos in readdTilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (readdTilemap.HasTile(localPlace))
                {
                    tilemap.SetTile(localPlace, readdTilemap.GetTile(localPlace));
                }
            }
            Destroy(readdTilemap.gameObject);


            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (tilemap.HasTile(localPlace))
                {
                    TileBase originalTile = tilemapToSlice.GetTile(localPlace);
                    if (originalTile == tilesetObjects.startTile)
                    {
                        startingTilemap = tilemap;
                    }
                    tilemap.SetTile(localPlace, originalTile);
                }
            }
        }
        SetCheckpoit(startingTilemap, new List<Tilemap>(workingTilemaps), Vector3Int.one);

        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = wallMap.GetTile(localPlace);
            if (tile == tilesetObjects.checkpointTile || tile == tilesetObjects.fakeWallTile)
            {
                wallMap.SetTile(localPlace, null);
            }
        }

    }
    void SetCheckpoit(Tilemap thisMap, List<Tilemap> mapsToSetCheckpoint, Vector3Int checkpoint)
    {
        foreach (var pos in thisMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (thisMap.GetTile(localPlace) == tilesetObjects.checkpointTile)
            {
                if (localPlace == checkpoint)
                {
                    // instantiate checkpoint
                    workingTilemaps.Remove(thisMap);
                }
                else
                {
                    thisMap.SetTile(localPlace, null);
                    foreach (Tilemap tilemap in mapsToSetCheckpoint)
                    {
                        if (tilemap.HasTile(localPlace))
                        {
                            SetCheckpoit(tilemap, new List<Tilemap>(workingTilemaps), localPlace);
                        }
                    }
                }
            }
        }
    }

    bool tileInMaps(List<Tilemap> maps, Vector3Int pos)
    {
        foreach (Tilemap tilemap in maps)
        {
            if (tilemap.HasTile(pos))
            {
                return true;
            }
        }
        return false;
    }

    bool HasNeighbor(Tilemap map, Vector3Int pos)
    {
        if (map.GetTile(pos + Vector3Int.right))
        {
            return true;
        }
        if (map.GetTile(pos + Vector3Int.left))
        {
            return true;
        }
        if (map.GetTile(pos + Vector3Int.up))
        {
            return true;
        }
        if (map.GetTile(pos + Vector3Int.down))
        {
            return true;
        }
        return false;
    }
}
