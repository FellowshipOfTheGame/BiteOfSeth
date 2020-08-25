using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTile : WorldTile {
    public Tile fakeTile = null;

    protected override bool IsNeighbour(Vector3Int position, ITilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && (tile == this || tile == fakeTile));
    }

    protected override Category calcSprite(bool[] neighbours, int count){
        switch (count){
            case 0:
                return GetConfig("noOpen");

            case 1:
                if (neighbours[0]) return GetConfig("upOpen");
                if (neighbours[1]) return GetConfig("downOpen");
                if (neighbours[2]) return GetConfig("leftOpen");
                return GetConfig("rightOpen");
            
            case 2:
                if(neighbours[0]){
                    if (neighbours[1]) return GetConfig("vert");
                    if (neighbours[2]) return GetConfig("upLeft");
                    return GetConfig("upRight");
                }
                if(neighbours[1]){
                    if (neighbours[2]) return GetConfig("downLeft");
                    return GetConfig("downRight");
                }
                return GetConfig("hor");

            case 3:
                if (!neighbours[0]) return GetConfig("upGnd");
                if (!neighbours[1]) return GetConfig("downGnd");
                if (!neighbours[2]) return GetConfig("leftGnd");
                return GetConfig("rightGnd");

            
            default:
                return GetConfig("main");
        }
    }
}
