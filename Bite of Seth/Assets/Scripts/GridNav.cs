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

    // Move function to be called on fixed updates, return a bool for if the movement finished;
    public static bool MoveToFixed(Rigidbody2D rigidbody, Vector2 targetPosition, float movementSpeed)
    {
        Vector2 increment = new Vector2(targetPosition.x, targetPosition.y) - rigidbody.position;
        if (increment.magnitude > 0.1f) // max diference between position and target position to consider
        {
            increment = increment.normalized * Time.fixedDeltaTime * movementSpeed;
            rigidbody.MovePosition(rigidbody.position + increment);
            return false;
        }
        else
        {
            rigidbody.MovePosition(targetPosition); // final move so the rigidbody ends exactly on the target point
            return true;
        }
    }

    public static List<GameObject> GetObjectsInPath(Rigidbody2D rigidbody, Vector2 desiredMovement)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(rigidbody.position, desiredMovement, desiredMovement.magnitude);
        return MakeListFromHits(hits, rigidbody.gameObject);
    }
    public static List<GameObject> GetObjectsInPath(Rigidbody2D rigidbody, Vector2 desiredMovement, LayerMask layerMask)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(rigidbody.position, desiredMovement, desiredMovement.magnitude, layerMask);
        return MakeListFromHits(hits, rigidbody.gameObject);
    }
    private static List<GameObject> MakeListFromHits(RaycastHit2D[] hits, GameObject casterObject)
    {
        List<GameObject> objectsInPath = new List<GameObject>();
        foreach (RaycastHit2D h in hits)
        {
            // ignores hit in colliders of the caster object
            GameObject obj = h.transform.gameObject;
            if (obj != casterObject)
            {
                objectsInPath.Add(obj);
            }            
        }
        return objectsInPath;
    }
}
