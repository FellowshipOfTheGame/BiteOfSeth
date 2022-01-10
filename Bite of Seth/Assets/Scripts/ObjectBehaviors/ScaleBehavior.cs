using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBehavior : MonoBehaviour
{

    public PlateBehavior plate1;
    public PlateBehavior plate2;
    private bool balanced = true;

    public float platesSpeed = 1f;

    private int movingPlates = 0;

    public LineRenderer line1;
    public LineRenderer line2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Balance();
        Vector3 var = new Vector3(0f, -0.5f, 0f);
        line1.SetPosition(1, plate1.transform.localPosition + var);
        line2.SetPosition(1, plate2.transform.localPosition + var);
    }

    public void Balance()
    {

        //if (balancing) return;

        float weight1 = plate1.GetWeight();
        float weight2 = plate2.GetWeight();

        bool canMove1 = false;
        bool canMove2 = false;

        if (weight1 > weight2) {
            
            canMove1 = plate1.CanMoveDown();
            canMove2 = plate2.CanMoveUp();

            if (canMove1 && canMove2) {
                movingPlates = 2;
                plate2.MoveUp(platesSpeed);
                plate1.MoveDown(platesSpeed);
                balanced = false;
            } else {
                balanced = true;
            }

        } else if (weight1 < weight2) {

            canMove1 = plate1.CanMoveUp();
            canMove2 = plate2.CanMoveDown();

            if (canMove1 && canMove2) {
                movingPlates = 2;
                plate1.MoveUp(platesSpeed);
                plate2.MoveDown(platesSpeed);
                balanced = false;
            } else {
                balanced = true;
            }

        } else {

            canMove1 = plate1.CanMoveBack();
            canMove2 = plate2.CanMoveBack();

            if (canMove1 && canMove2) {
                movingPlates = 2;
                plate1.MoveBack(platesSpeed);
                plate2.MoveBack(platesSpeed);
                balanced = false;
            } else {
                balanced = true;
            }

        }
    }

    public bool IsBalanced()
    {
        return balanced;
    }

    public void PlateStopped()
    {
        movingPlates--;
        if(movingPlates == 0) {
            Balance();
        }
    }

    public void Reset()
    {
        enabled = false;
        movingPlates = 0;
        plate1.Reset();
        plate2.Reset();
        enabled = true;
    }

}
