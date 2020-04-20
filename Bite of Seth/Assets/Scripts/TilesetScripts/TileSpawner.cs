using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{
    public Tilemap tilemap;
    public TileObject[] tileObjects = null;

    private void Start()
    {
        spawnObjects();
    }

    private void spawnObjects()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = tilemap.GetTile(localPlace);
            if (tile != null)
            {
                for (int i = 0; i < tileObjects.Length; i++)
                {
                    if (tile == tileObjects[i].tile)
                    {
                        if (tileObjects[i].objectToSpawn != null)
                        {
                            Vector3 objectPlace = localPlace + tilemap.layoutGrid.cellSize / 2;
                            Instantiate(tileObjects[i].objectToSpawn, objectPlace, Quaternion.identity, tilemap.gameObject.transform);
                        }
                        tilemap.SetTile(localPlace, null);
                        break;
                    }
                }
            }            
        }
    }
}
