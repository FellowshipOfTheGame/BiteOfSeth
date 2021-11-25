using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateBehavior : MonoBehaviour
{

    private Vector2 detectPos;
    private int weight = 0;
    public LayerMask weightMask;
    public LayerMask collisionMask;

    public GameObject tempCol;
    private GameObject tcInstance = null;
    private Vector2 tcPos;
    
    private Rigidbody2D rb;

    private int moves = 0;
    private bool movingUp = false;

    private ScaleBehavior scale;

    private Vector2 lookingDirection;
    private Vector2 targetPosition;
    public bool isMoving = false;
    private float speed;
    private GameManager gameManager;

    private List<GameObject> objects;
    private List<GameObject> StopObjects;

    private Vector2 initialPosition;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        scale = transform.parent.GetComponent<ScaleBehavior>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialPosition = rb.position;
    }

    public void StartMovement(Vector2 desiredMovement, float _speed)
    {

        // return if movement blocked by game manager
        if (ServiceLocator.Get<GameManager>().lockMovement > 0) {
            return;
        }

        lookingDirection = desiredMovement.normalized;

        targetPosition = GridNav.WorldToGridPosition(rb.position) + desiredMovement;

        isMoving = true;
        speed = _speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        detectPos = rb.position + GridNav.down;
        objects = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(detectPos), (weight + 1) * GridNav.up, weightMask, gameObject);
        weight = objects.Count;

        //Debug.Log(transform.name + ": weight = " + weight);

        if (isMoving) {
            
            // isMoving == true
            // return if movement blocked by game manager
            if (gameManager.lockMovement > 0) {
                return;
            }

            isMoving = !GridNav.MoveToFixed(rb, targetPosition, speed);

            if (isMoving == false) {
                StoppedMoving();
            }

        }

    }

    public void StoppedMoving()
    {
        SetTemporaryCollider(false, true);

        if (movingUp) {
            moves++;
        } else {
            moves--;
        }
        
        for (int i = 0; i < StopObjects.Count; i++) {
            StopObjects[i].GetComponent<FallBehavior>().Activate();
        }

    }

    private void SetTemporaryCollider(bool active, bool up)
    {
        if (active) {
            //Put a temporary collider
            if (up) {
                tcPos = rb.position;
            } else {
                tcPos = rb.position + GridNav.down;
            }
            if (tcInstance != null) {
                Destroy(tcInstance);
            }
            tcInstance = Instantiate(tempCol, tcPos, Quaternion.identity) as GameObject;
            tcInstance.transform.parent = gameObject.transform;
        } else {
            if (tcInstance != null) {
                Destroy(tcInstance);
            }
        }
    }

    public void MoveUp(float _speed)
    {

        speed = _speed;

        movingUp = true;

        if(weight == 0) SetTemporaryCollider(true, true);

        //insertion sort objects list based on pos.y
        SortByYPos(objects);

        float time = 0f;

        StopObjects = objects;

        for (int i = objects.Count - 1; i >= 0; i--) {
            //Debug.Log(transform.name + " " + objects[i].transform.position.y);
            //objects[i].GetComponent<FallBehavior>().PushUpAfterXSeconds(speed, time);
            objects[i].GetComponent<FallBehavior>().PushUp(speed);
            time += 0.1f;
        }
        
        StartMovement(GridNav.up, speed);

    }

    private void SortByYPos(List<GameObject> objects)
    {
        for (int i = 1; i < objects.Count; i++) {
            int j = i;
            while (j > 0 && objects[j].transform.position.y < objects[j - 1].transform.position.y) {
                GameObject aux = objects[j];
                objects[j] = objects[j - 1];
                objects[j - 1] = aux;
                j -= 1;
            }
        }
    }

    public void MoveDown(float _speed)
    {

        speed = _speed;

        movingUp = false;
        SetTemporaryCollider(true, false);

        //insertion sort objects list based on pos.y
        SortByYPos(objects);

        StopObjects = objects;

        StartMovement(GridNav.down, speed);

        for (int i = 0; i < objects.Count; i++) {
            objects[i].GetComponent<FallBehavior>().PushDown(speed);
        }

    }

    public void MoveBack(float speed)
    {
        if (moves > 0) {
            MoveDown(speed);
        } else if(moves < 0){
            MoveUp(speed);
        }
    }

    public bool CanMoveUp()
    {
        if (isMoving) return false;
        
        /*foreach (GameObject o in oip) {
            Debug.Log(transform.name + " UP " + o);
        }*/

        for (int i = objects.Count - 1; i >= 0; i--) {
            if (objects[i].GetComponent<Movable>().isMoving) return false;
        }

        List<GameObject> oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(rb.position + ((weight) * GridNav.up)), GridNav.up / 3, collisionMask, gameObject);

        return (oip.Count == 0);
    }

    public bool CanMoveDown()
    {
        if (isMoving) return false;

        /*foreach (GameObject o in oip) {
            Debug.Log(transform.name + " DOWN " + o);
        }*/

        for (int i = objects.Count - 1; i >= 0; i--) {
            if (objects[i].GetComponent<Movable>().isMoving) return false;
        }

        List<GameObject> oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(rb.position + GridNav.down), GridNav.down/3, collisionMask, gameObject);
        
        return (oip.Count == 0);
    }

    public bool CanMoveBack()
    {
        if (isMoving) return false;
        if (moves > 0) {
            return CanMoveDown();
        }else if (moves < 0) {
            return CanMoveUp();
        }
        return false;
    }

    public float GetWeight()
    {
        return weight;
    }

    public void Reset()
    {
        enabled = false;
        isMoving = false;
        SetTemporaryCollider(false, true);
        rb.position = initialPosition;
        weight = 0;
        moves = 0;
        enabled = true;
        Debug.Log(transform.name);
    }

    public int GetMoves()
    {
        return moves;
    }

}
