using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldSwitcher : MonoBehaviour {

    public static int world = 0;

    List<RuleTile> walls;
    public Tilemap map;

    // Start is called before the first frame update
    void Start() {
        walls = new List<RuleTile>();

        foreach (var pos in map.cellBounds.allPositionsWithin) {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            RuleTile tile = (RuleTile) map.GetTile(localPlace);
            if (tile != null){
                walls.Add(tile);
                tile.ChangeWorld(0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
