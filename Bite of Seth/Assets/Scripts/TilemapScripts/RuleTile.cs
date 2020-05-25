using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTile : Tile {
    //[HideInInspector] public bool updated = false;
    public Sprite middle;
    public Sprite[] oneSide;
    public Sprite[] twoSide;

    public bool[,] neighbours;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
        for (int yd = -1; yd <= 1; yd++){
            for (int xd = -1; xd <= 1; xd++){
                Vector3Int location = new Vector3Int(position.x + xd, position.y + yd, position.z);
                if (IsNeighbour(location, tilemap))   
                    tilemap.RefreshTile(location);  
            }
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        neighbours = new bool[3,3];
        int count = 0;

        for (int yd = -1; yd <= 1; yd++){
            for (int xd = -1; xd <= 1; xd++){
                if (xd != 0 || yd != 0){
                    Vector3Int location = new Vector3Int(position.x + xd, position.y + yd, position.z);
                    if (IsNeighbour(location, tilemap)){
                        this.neighbours[1+xd,1-yd] = true;
                        count++;
                    }
                }     
            }
        }
        
        tileData.sprite = calcSprite(neighbours, count);  
    }

    bool IsNeighbour(Vector3Int position, ITilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && tile == this);
    }
  
    Sprite calcSprite(bool[,] neighbours, int count){
        if (!neighbours[1,0]) return oneSide[0];
        
        return middle;
    }
}
