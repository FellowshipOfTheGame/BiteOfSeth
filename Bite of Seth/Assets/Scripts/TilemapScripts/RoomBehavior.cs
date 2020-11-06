using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomBehavior : MonoBehaviour
{
    [SerializeField] private Tilemap backupTilemap = null;
    [SerializeField] private TilesetObjects tilesetObjects = null;

    public void SpawnRoom(TilesetObjects _tilesetObjects)
    {
        tilesetObjects = _tilesetObjects;
        SpawnRoom();
    }

    public void SpawnRoom()
    {
        bool isFirstSpawn = false;
        if (backupTilemap == null)
        {
            backupTilemap = GetComponentInChildren<Tilemap>();
            backupTilemap.gameObject.SetActive(false);
            isFirstSpawn = true;
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
                            GameObject g = Instantiate(tilesetObjects.objectsToSpawn[i].objectToSpawn, objectPlace, Quaternion.identity, gameObject.transform) as GameObject;
                            Debug.Log("Spawnou " + g.ToString());
                            if (tile == tilesetObjects.checkpointTile)
                            {
                                if (isFirstSpawn)
                                {
                                    g.GetComponent<CheckpointBehavior>().SetRoom(this);
                                }
                                else
                                {
                                    DestroyImmediate(g);
                                }                                
                            }
                        }
                        tempTilemap.SetTile(localPlace, null);
                        break;
                    }
                }
            }
        }
        DestroyImmediate(tempTilemap.gameObject);
    }

    public void DespawnRoom()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Desespawnou " + child.ToString());
            if (child.GetComponent<CheckpointBehavior>() != null)
            {
                // ignore and don't destroy the checkpoint object
            }
            else if (child.gameObject == backupTilemap.gameObject)
            {
                // ignore and don't destroy the backup tilemap object
            }
            else if (child.GetComponent<CollectableBehavior>() != null)
            {
                child.GetComponent<CollectableBehavior>().Decollect();
                Destroy(child.gameObject);
            }
            else
            {
                Debug.Log(child.gameObject);
                Destroy(child.gameObject);
            }
        }
    }
}
