using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu()]
public class TilesetObjects: ScriptableObject
{
    [Header("Basic funcionality tiles")]
    public Tile wallTile = null;
    public Tile logicWallTile = null;
    public Tile checkpointTile = null;
    public Tile logicCheckpointTile = null;
    public Tile startTile = null;
    public Tile fakeWallTile = null;
    [Header("Objects to spawn by tile")]
    public TileObject[] objectsToSpawn = null;
}

[System.Serializable]
public class TileObject
{
    public TileBase tile = null;
    public UnityEngine.Object objectToSpawn = null;
}