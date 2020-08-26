using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class RuleTile : CustomTile {
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
                if (!neighbours[1]) {
                    if (neighbours[6]){
                        if (neighbours[8]) return GetConfig("upFlat");
                        return GetConfig("qHor");
                    }

                    if (neighbours[8]) return GetConfig("dHor");
                    return GetConfig("upGnd");
                }
                if (!neighbours[7]) {
                    if (neighbours[0]) {
                        if (neighbours[2]) return GetConfig("downFlat");
                        return GetConfig("pHor");
                    }

                    if (neighbours[2]) return GetConfig("bHor");
                    return GetConfig("downGnd");
                }
                if (!neighbours[3]) {
                    if (neighbours[2]) {
                        if (neighbours[8]) return GetConfig("leftFlat");
                        return GetConfig("pVert");
                    }

                    if (neighbours[6]) return GetConfig("bVert");
                    return GetConfig("leftGnd");
                }
                if (!neighbours[5]) {
                    if (neighbours[0]) {
                        if (neighbours[6]) return GetConfig("rightFlat");
                        return GetConfig("qVert");
                    }

                    if(neighbours[6]) return GetConfig("dVert");
                    return GetConfig("rightGnd");
                }
                return GetConfig("noOpen");

            case 4:
                if (neighbours[0]) {
                    if(neighbours[2]) {
                        if(neighbours[6]) {
                            if (neighbours[8]) return GetConfig("main");
                            return GetConfig("corner2");
                        }
                        if(neighbours[8]) return GetConfig("corner3");
                        return GetConfig("upJoint");
                    }

                    if(neighbours[6]) {
                        if (neighbours[8]) return GetConfig("corner1");
                        return GetConfig("leftJoint");
                    }

                    if (neighbours[8]) return GetConfig("diag2");
                    return GetConfig("peak4");
                }

                if (neighbours[2]) {
                    if (neighbours[6]) {
                        if (neighbours[8]) return GetConfig("corner4");
                        return GetConfig("diag1");
                    }
                    if (neighbours[8]) return GetConfig("rightJoint");
                    return GetConfig("peak1");
                }

                if (neighbours[6]) {
                    if (neighbours[8]) return GetConfig("downJoint");
                    return GetConfig("peak3");
                }

                if (neighbours[8]) return GetConfig("peak2");
                return GetConfig("cross");

            default:
                //Debug.Log("4n");
                return GetConfig("main");
        }
    }
}
