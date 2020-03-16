using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFunctions : MonoBehaviour
{
    public Tilemap tilemapReference = null;
    public Tile wallTile = null;
    public Tile checkpointTile = null;
    // Start is called before the first frame update
    void Start()
    {
        SliceTilemap(tilemapReference);
    }

    void SliceTilemap(Tilemap tilemapToSlice)
    {
        List<Tilemap> slicedTilemaps = new List<Tilemap>();

        tilemapReference.gameObject.SetActive(false);

        Tilemap wallMap = Instantiate(tilemapToSlice, transform);
        wallMap.gameObject.SetActive(false);

        // turn checkpoints to walls then clear non-walls
        foreach (var pos in wallMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = wallMap.GetTile(localPlace);
            if (tile != wallTile && tile != checkpointTile)
            {
                wallMap.SetTile(localPlace, null);
            }
            else
            {
                //wallMap.SetTile(localPlace, wallTile);
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
                    slicedTilemaps.Add(newRoom);
                }
            }            
        }

        // clear wallmap from rooms
        foreach(Tilemap tilemap in slicedTilemaps)
        {
            foreach (var pos in wallMap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                if (tilemap.HasTile(localPlace))
                {
                    tilemap.SetTile(localPlace, tilemapToSlice.GetTile(localPlace));
                    if (wallMap.GetTile(localPlace) == wallTile)
                    {
                        tilemap.SetTile(localPlace, null);
                    }
                }
            }
        }

        //Destroy(wallMap.gameObject);
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
}
