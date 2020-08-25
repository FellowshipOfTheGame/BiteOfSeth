using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTile : Tile {

    [System.Serializable]
    public class Sample {
        public string label;
        public Sprite[] art;
    }

    [System.Serializable]
    public class Category {
        public string title;
        public Sample[] samples;
    }

    public Category[] categories;

    Category config;


    public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
        base.RefreshTile(position, tilemap);

        for (int yd = -1; yd <= 1; yd++) {
            Vector3Int location = new Vector3Int(position.x, position.y + yd, position.z);
            if (IsNeighbour(location, tilemap))
                tilemap.RefreshTile(location);
        }

        for (int xd = -1; xd <= 1; xd++) {
            Vector3Int location = new Vector3Int(position.x + xd, position.y, position.z);
            if (IsNeighbour(location, tilemap))
                tilemap.RefreshTile(location);
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
        base.GetTileData(position, tilemap, ref tileData);

        bool[] neighbours = new bool[4];
        int count = 0;

        Vector3Int location = new Vector3Int(position.x, position.y + 1, position.z);
        if (IsNeighbour(location, tilemap)) {
            neighbours[0] = true;
            count++;
        }

        location = new Vector3Int(position.x, position.y - 1, position.z);
        if (IsNeighbour(location, tilemap)) {
            neighbours[1] = true;
            count++;
        }

        location = new Vector3Int(position.x - 1, position.y, position.z);
        if (IsNeighbour(location, tilemap)) {
            neighbours[2] = true;
            count++;
        }

        location = new Vector3Int(position.x + 1, position.y, position.z);
        if (IsNeighbour(location, tilemap)) {
            neighbours[3] = true;
            count++;
        }


        config = calcSprite(neighbours, count);
        tileData.sprite = config.samples[0].art[0];

    }

    protected virtual bool IsNeighbour(Vector3Int position, ITilemap tilemap) {
        TileBase tile = tilemap.GetTile(position);
        return (tile != null && tile == this);
    }

    protected virtual Category calcSprite(bool[] neighbours, int count) {
        return GetConfig("main"); 
    }

    protected Category GetConfig(string name){
        foreach(Category c in categories){
            if (c.title == name) return c;
        }

        return categories[0];
    }
}
