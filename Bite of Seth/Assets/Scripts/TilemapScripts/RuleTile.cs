using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTile : Tile {

    public Tile fakeTile = null;
    //[HideInInspector] public bool updated = false;
    public Sprite[] main, upGnd, downGnd, leftGnd, rightGnd;
    public Sprite[] vert, hor, upLeft, upRight, downRight, downLeft;
    public Sprite[] noOpen, upOpen, downOpen, leftOpen, rightOpen;


    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
        base.RefreshTile(position, tilemap);

        for (int yd = -1; yd <= 1; yd++){
            Vector3Int location = new Vector3Int(position.x, position.y + yd, position.z);
            if (IsNeighbour(location, tilemap))   
                tilemap.RefreshTile(location);  
        }

        for (int xd = -1; xd <= 1; xd++){
            Vector3Int location = new Vector3Int(position.x + xd, position.y, position.z);
            if (IsNeighbour(location, tilemap))   
                tilemap.RefreshTile(location);  
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){
        base.GetTileData(position, tilemap, ref tileData);

        bool[] neighbours = new bool[4];
        int count = 0;

        Vector3Int location = new Vector3Int(position.x, position.y + 1, position.z);
        if (IsNeighbour(location, tilemap)){
            neighbours[0] = true;
            count++;
        }

        location = new Vector3Int(position.x, position.y - 1, position.z);
        if (IsNeighbour(location, tilemap)){
            neighbours[1] = true;
            count++;
        }

        location = new Vector3Int(position.x - 1, position.y, position.z);
        if (IsNeighbour(location, tilemap)){
            neighbours[2] = true;
            count++;
        }

        location = new Vector3Int(position.x + 1, position.y, position.z);
        if (IsNeighbour(location, tilemap)){
            neighbours[3] = true;
            count++;
        }
        
        
        tileData.sprite = calcSprite(neighbours, count);

    }

    bool IsNeighbour(Vector3Int position, ITilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && (tile == this || tile == fakeTile));
    }
  
    Sprite calcSprite(bool[] neighbours, int count){
        switch (count){
            case 0:
                return noOpen[0];

            case 1:
                if (neighbours[0]) return upOpen[0];
                if (neighbours[1]) return downOpen[0];
                if (neighbours[2]) return leftOpen[0];
                return rightOpen[0];
            
            case 2:
                if(neighbours[0]){
                    if (neighbours[1]) return vert[0];
                    if (neighbours[2]) return upLeft[0];
                    return upRight[0];
                }
                if(neighbours[1]){
                    if (neighbours[2]) return downLeft[0];
                    return downRight[0];
                }
                return hor[0];

            case 3:
                if (!neighbours[0]) return upGnd[0];
                if (!neighbours[1]) return downGnd[0];
                if (!neighbours[2]) return leftGnd[0];
                return rightGnd[0];

            
            default:
                return main[0];
        }
    }
}
