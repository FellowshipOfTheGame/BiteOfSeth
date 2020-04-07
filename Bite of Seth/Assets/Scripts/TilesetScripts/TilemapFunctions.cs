using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFunctions : MonoBehaviour
{
    public Tilemap tilemapReference = null;
    public Tile wallTile = null;
    public Tile checkpointTile = null;
    public Tile startTile = null;
    List<Tilemap> slicedTilemaps = null;
    // Start is called before the first frame update
    void Start()
    {
        SliceTilemap(tilemapReference);
    }

    void SliceTilemap(Tilemap tilemapToSlice)
    {
        slicedTilemaps = new List<Tilemap>();

        tilemapReference.gameObject.SetActive(false);

        Tilemap wallMap = Instantiate(tilemapToSlice, transform);
        wallMap.gameObject.SetActive(true);
        wallMap.gameObject.name = "Walls tilemap";

        // turn checkpoints to walls then clear non-walls
        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = wallMap.GetTile(localPlace);
            if (tile != wallTile && tile != checkpointTile)
            {
                wallMap.SetTile(localPlace, null);
            }
        }
        
        // create tilemaps for each room with FloodFill
        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (true)
            {
                if (tileInMaps(slicedTilemaps, localPlace) == false)
                {
                    Tilemap newRoom = Instantiate(wallMap, transform);
                    newRoom.FloodFill(localPlace, wallTile);
                    newRoom.gameObject.SetActive(true);
                    newRoom.gameObject.name = "room " + slicedTilemaps.Count.ToString();
                    slicedTilemaps.Add(newRoom);
                }
            }            
        }

        Tilemap startingTilemap = null;

        // clear wallmap from rooms
        foreach(Tilemap tilemap in slicedTilemaps)
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
                    if (originalTile == startTile)
                    {
                        startingTilemap = tilemap;
                    }
                    tilemap.SetTile(localPlace, originalTile);                    
                }
            }
        }
        SetCheckpoit(startingTilemap,new List<Tilemap>(slicedTilemaps), Vector3Int.one);

        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (wallMap.GetTile(localPlace) == checkpointTile)
            {
                wallMap.SetTile(localPlace, null);
            }
        }

    }
    void SetCheckpoit(Tilemap thisMap,List<Tilemap> mapsToSetCheckpoint, Vector3Int checkpoint)
    {
            foreach (var pos in thisMap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (thisMap.GetTile(localPlace) == checkpointTile)
                {
                    if(localPlace == checkpoint)
                    {
                    // instantiate checkpoint
                    slicedTilemaps.Remove(thisMap);
                    }
                    else
                    {
                        thisMap.SetTile(localPlace, null);
                        foreach (Tilemap tilemap in mapsToSetCheckpoint)
                        {
                            if (tilemap.HasTile(localPlace))
                            {
                            SetCheckpoit(tilemap, new List<Tilemap>(slicedTilemaps), localPlace);
                            }                            
                        }
                    }      
                }
            }
    }

    bool tileInMaps(List<Tilemap> maps, Vector3Int pos)
    {
        foreach(Tilemap tilemap in maps)
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