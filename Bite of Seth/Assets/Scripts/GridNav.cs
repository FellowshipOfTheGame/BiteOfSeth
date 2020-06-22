using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridNav
{
    private static Grid _grid;
    private static Grid grid
    {
        get
        {
            // lazy reference for the grid object
            if (_grid == null)
            {
                Grid g = Object.FindObjectOfType<Grid>();
                if (g == null)
                {
                    Debug.LogError("GridNav couldn't find a Grid");
                }
                _grid = g;                
            }
            return _grid;
        }
        set
        {
            _grid = value;
        }
    }
    private static Vector2 gridSize
    {
        get
        {
            return grid.cellSize;
        }
    }
    private static Vector2 gridOffset
    {
        get
        {
            return grid.transform.position + grid.cellSize/2;
        }
    }
    public static Vector2 right
    {
        get
        {
            return new Vector2(gridSize.x,0);
        }
    }
    public static Vector2 left
    {
        get
        {
            return new Vector2(-gridSize.x, 0);
        }
    }
    public static Vector2 up
    {
        get
        {
            return new Vector2(0, gridSize.y);
        }
    }
    public static Vector2 down
    {
        get
        {
            return new Vector2(0, -gridSize.y);
        }
    }

    public static Vector2 WorldToGridPosition(Vector2 position)
    {
        // round the position to snap to the center of a grid cell
        position.x = gridSize.x * Mathf.Floor(position.x / gridSize.x) + gridOffset.x;
        position.y = gridSize.y * Mathf.Floor(position.y / gridSize.y) + gridOffset.y;
        return position;
    }

    /*public static Vector2 GridToWorldPosition(Vector2 position)
    {
        // round the position to snap to the center of a grid cell
        position.x = (position.x - gridOffset.x)
        position.y = gridSize.y * Mathf.Floor(position.y / gridSize.y) + gridOffset.y;
        return position;
    }*/

    // Move function to be called on fixed updates, return a bool for if the movement finished;
    public static bool MoveToFixed(Rigidbody2D rigidbody, Vector2 targetPosition, float movementSpeed)
    {
        Vector2 positionDifferece = new Vector2(targetPosition.x, targetPosition.y) - rigidbody.position;
        if (positionDifferece == Vector2.zero)
        {
            return true;
        }
        Vector2 increment = positionDifferece.normalized * Time.fixedDeltaTime * movementSpeed;
        if (increment.sqrMagnitude < positionDifferece.sqrMagnitude)
        {            
            rigidbody.MovePosition(rigidbody.position + increment);
            return false;
        }
        else
        {
            rigidbody.MovePosition(targetPosition); // final move so the rigidbody ends exactly on the target point
            return true;
        }
    }

    public static List<GameObject> GetObjectsInPath(Vector2 origin, Vector2 desiredMovement, params GameObject[] objectsToIgnore)
    {

        HashSet<RaycastHit2D> allHits = new HashSet<RaycastHit2D>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, desiredMovement, desiredMovement.magnitude);
        Debug.DrawRay(origin, desiredMovement, Color.red);
        foreach (RaycastHit2D h in hits) {
            allHits.Add(h);
        }

        Vector2 desiredMovement2 = Vector2.zero, desiredMovement3 = Vector2.zero;

        if (desiredMovement.y == 0) {
            desiredMovement2 = new Vector2(desiredMovement.x, (desiredMovement.x / 2) * 0.9f);
            desiredMovement3 = new Vector2(desiredMovement.x, (-desiredMovement.x / 2) * 0.9f);
        } else if (desiredMovement.x == 0) {
            desiredMovement2 = new Vector2((desiredMovement.y / 2) * 0.9f, desiredMovement.y);
            desiredMovement3 = new Vector2((-desiredMovement.y / 2) * 0.9f, desiredMovement.y);
        }

        RaycastHit2D[] hits2 = Physics2D.RaycastAll(origin, desiredMovement2, desiredMovement2.magnitude);
        Debug.DrawRay(origin, desiredMovement2, Color.red);
        foreach (RaycastHit2D h in hits2) {
            allHits.Add(h);
        }

        RaycastHit2D[] hits3 = Physics2D.RaycastAll(origin, desiredMovement3, desiredMovement3.magnitude);
        Debug.DrawRay(origin, desiredMovement3, Color.red);
        foreach (RaycastHit2D h in hits3) {
            allHits.Add(h);
        }

        return MakeListFromHits(allHits, objectsToIgnore);
    }

    public static List<GameObject> GetObjectsInPath(Vector2 origin, Vector2 desiredMovement, LayerMask layerMask, params GameObject[] objectsToIgnore)
    {

        HashSet<RaycastHit2D> allHits = new HashSet<RaycastHit2D>();

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, desiredMovement, desiredMovement.magnitude, layerMask);
        Debug.DrawRay(origin, desiredMovement, Color.blue);
        foreach (RaycastHit2D h in hits) {
            allHits.Add(h);
        }

        Vector2 desiredMovement2 = Vector2.zero, desiredMovement3 = Vector2.zero;

        if (desiredMovement.y == 0) {
            desiredMovement2 = new Vector2(desiredMovement.x, (desiredMovement.x / 2) * 0.9f);
            desiredMovement3 = new Vector2(desiredMovement.x, (-desiredMovement.x / 2) * 0.9f);
        } else if (desiredMovement.x == 0) {
            desiredMovement2 = new Vector2((desiredMovement.y / 2) * 0.9f, desiredMovement.y);
            desiredMovement3 = new Vector2((-desiredMovement.y / 2) * 0.9f, desiredMovement.y);
        }

        RaycastHit2D[] hits2 = Physics2D.RaycastAll(origin, desiredMovement2, desiredMovement2.magnitude, layerMask);
        Debug.DrawRay(origin, desiredMovement2, Color.blue);
        foreach (RaycastHit2D h in hits2) {
            allHits.Add(h);
        }

        RaycastHit2D[] hits3 = Physics2D.RaycastAll(origin, desiredMovement3, desiredMovement3.magnitude, layerMask);
        Debug.DrawRay(origin, desiredMovement3, Color.blue);
        foreach (RaycastHit2D h in hits3) {
            allHits.Add(h);
        }
        
        return MakeListFromHits(allHits, objectsToIgnore);
    }
    private static List<GameObject> MakeListFromHits(HashSet<RaycastHit2D> hits, GameObject[] objectsToIgnore)
    {
        List<GameObject> objectsInPath = new List<GameObject>();
        HashSet<GameObject> aux = new HashSet<GameObject>();
        foreach (RaycastHit2D h in hits)
        {
            if (aux.Add(h.transform.gameObject)) {
                objectsInPath.Add(h.transform.gameObject);
            }
        }
        foreach (GameObject o in objectsToIgnore)
        {
            objectsInPath.Remove(o);
        }
        return objectsInPath;
    }
}
