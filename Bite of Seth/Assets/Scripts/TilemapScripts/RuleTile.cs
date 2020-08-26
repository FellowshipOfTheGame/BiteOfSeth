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

    protected override Category calcSprite(bool[] neighbours){

        int cross = 0;
        if (neighbours[1]) cross++;
        if (neighbours[7]) cross++;
        if (neighbours[3]) cross++;
        if (neighbours[5]) cross++;


        switch (cross){
            case 0:
                return GetConfig("noOpen");

            case 1:
                if (neighbours[1]) return GetConfig("upOpen");
                if (neighbours[7]) return GetConfig("downOpen");
                if (neighbours[3]) return GetConfig("leftOpen");
                if (neighbours[5]) return GetConfig("rightOpen");
                return GetConfig("noOpen");
            
            case 2:
               // Debug.Log("2n");
                if(neighbours[1]){
                    if (neighbours[7]) {
                        return GetConfig("vert");
                    }
                    if (neighbours[3]) {
                        if (neighbours[0]) return GetConfig("quad4");
                        return GetConfig("upLeft");
                    }
                    if (neighbours[5]) {
                        if (neighbours[2]) return GetConfig("quad1");
                        return GetConfig("upRight");
                    }
                    return GetConfig("noOpen");
                }
                if(neighbours[7]){
                    if (neighbours[3]) {
                        if (neighbours[6]) return GetConfig("quad3");
                        return GetConfig("downLeft");
                    }
                    if (neighbours[5]) {
                        if (neighbours[8]) return GetConfig("quad2");
                        return GetConfig("downRight");
                    }
                    return GetConfig("downOpen");
                }
                if(neighbours[3]){
                    if (neighbours[5]) return GetConfig("hor");
                    return GetConfig("leftOpen");
                }

                if(neighbours[5]){
                    return GetConfig("rightOpen");
                }
                return GetConfig("noOpen");

            case 3:
                if (!neighbours[1]) return GetConfig("upGnd");
                if (!neighbours[7]) return GetConfig("downGnd");
                if (!neighbours[3]) return GetConfig("leftGnd");
                if (!neighbours[5]) return GetConfig("rightGnd");
                return GetConfig("noOpen");

            case 4:
                return GetConfig("main");

            default:
                //Debug.Log("4n");
                return GetConfig("main");
        }
    }
}
