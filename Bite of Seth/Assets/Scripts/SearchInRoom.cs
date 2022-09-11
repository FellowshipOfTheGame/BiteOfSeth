using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchInRoom : MonoBehaviour
{

    public GameObject finderPrefab;
    public int roomMaxSizeX = 300;
    public int roomMaxSizeY = 300;

    public List<PortalEnergy> GetListOfPortalEnergy()
    {

        List<PortalEnergy> energies = new List<PortalEnergy>();

        Queue<GameObject> queue = new Queue<GameObject>();

        GameObject finder = Instantiate(finderPrefab, transform.position, Quaternion.identity);
        queue.Enqueue(finder);

        bool[,] visited = new bool[roomMaxSizeX, roomMaxSizeY];
        for (int i = 0; i < roomMaxSizeX; i++)
            for (int j = 0; j < roomMaxSizeY; j++)
                visited[i, j] = false;

        Queue<int> posXQueue = new Queue<int>();
        posXQueue.Enqueue(roomMaxSizeX / 2);
        int curPosX;

        Queue<int> posYQueue = new Queue<int>();
        posYQueue.Enqueue(roomMaxSizeY / 2);
        int curPosY;

        GameObject cur;

        // BFS para encontrar as energias
        while (queue.Count > 0) {

            cur = queue.Dequeue();
            curPosX = posXQueue.Dequeue();
            curPosY = posYQueue.Dequeue();

            // Marca a posição atual como já visitada
            visited[curPosX, curPosY] = true;
           
            Vector2[] directions = new Vector2[4] { GridNav.up, GridNav.right, GridNav.down, GridNav.left };
            for(int i=0; i<4; i++) {

                int dirX = (curPosX + (int)directions[i].x);
                int dirY = (curPosY + (int)directions[i].y);

                if (dirX >= 0 &&
                    dirY >= 0 &&
                    dirX < roomMaxSizeX &&
                    dirY < roomMaxSizeY &&
                    visited[dirX, dirY] == false) {

                    List<GameObject> oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(cur.transform.position), directions[i], gameObject);

                    bool isOnWall = false;
                    foreach (GameObject go in oip) {
                        if (go.tag == "LogicEnergy") {
                            PortalEnergy energy = go.GetComponent<PortalEnergy>();
                            if (energy != null && !energies.Contains(energy)) energies.Add(energy);
                        } else if (go.layer == 9) {
                            // wall é layer 9
                            isOnWall = true;
                        }
                    }

                    if (!isOnWall) {
                        GameObject newFinder = Instantiate(finderPrefab, cur.transform.position + (Vector3)directions[i], Quaternion.identity) as GameObject;
                        queue.Enqueue(newFinder);
                        visited[dirX, dirY] = true;
                        posXQueue.Enqueue(dirX);
                        posYQueue.Enqueue(dirY);
                    }

                }

            }
            
            // Destrói o objeto
            DestroyImmediate(cur);

        }

        return energies;
    }

}
