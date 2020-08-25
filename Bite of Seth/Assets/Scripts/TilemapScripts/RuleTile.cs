using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTile : Tile {
    public Tile fakeTile = null;
    Vector3Int pos;
    ITilemap map;
    Sprite[] config = null;

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
        pos = position;
        map = tilemap;

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
        
        
        config = calcSprite(neighbours, count);
        tileData.sprite = config[0];

    }

    bool IsNeighbour(Vector3Int position, ITilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && (tile == this || tile == fakeTile));
    }
  
    Sprite[] calcSprite(bool[] neighbours, int count){
        switch (count){
            case 0:
                return noOpen;

            case 1:
                if (neighbours[0]) return upOpen;
                if (neighbours[1]) return downOpen;
                if (neighbours[2]) return leftOpen;
                return rightOpen;
            
            case 2:
                if(neighbours[0]){
                    if (neighbours[1]) return vert;
                    if (neighbours[2]) return upLeft;
                    return upRight;
                }
                if(neighbours[1]){
                    if (neighbours[2]) return downLeft;
                    return downRight;
                }
                return hor;

            case 3:
                if (!neighbours[0]) return upGnd;
                if (!neighbours[1]) return downGnd;
                if (!neighbours[2]) return leftGnd;
                return rightGnd;

            
            default:
                return main;
        }
    }
}
