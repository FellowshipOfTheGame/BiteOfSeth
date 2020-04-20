using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomBehavior : MonoBehaviour
{
    private Tilemap backupTilemap = null;
    private TilesetObjects tilesetObjects = null;
    private Tilemap spawnedTilemap = null;

    public void SpawnRoom(TilesetObjects _tilesetObjects)
    {
        tilesetObjects = _tilesetObjects;
        SpawnRoom();
    }

    public void SpawnRoom()
    {
        if (backupTilemap == null)
        {
            backupTilemap = GetComponentInChildren<Tilemap>();
            backupTilemap.gameObject.SetActive(false);
        }

        Tilemap tempTilemap = Instantiate(backupTilemap, transform);

        foreach (var pos in tempTilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            TileBase tile = tempTilemap.GetTile(localPlace);
            if (tile != null)
            {
                for (int i = 0; i < tilesetObjects.objectsToSpawn.Length; i++)
                {
                    if (tile == tilesetObjects.objectsToSpawn[i].tile)
                    {
                        if (tilesetObjects.objectsToSpawn[i].objectToSpawn != null)
                        {
                            Vector3 objectPlace = localPlace + tempTilemap.layoutGrid.cellSize / 2;
                            Instantiate(tilesetObjects.objectsToSpawn[i].objectToSpawn, objectPlace, Quaternion.identity, gameObject.transform);
                        }
                        tempTilemap.SetTile(localPlace, null);
                        break;
                    }
                }
            }
        }
        Destroy(tempTilemap.gameObject);
    }
}
