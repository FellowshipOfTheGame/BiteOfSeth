using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    public enum Id { Red = 0, Blue = 1, Green = 2, Yellow = 3 };

    private Id[] statuesCorrectOrder = new Id[4];
    private Id[] statuesSelectedOrder = new Id[4];
    int nSelected = 0;

    public static PuzzleManager instance;
    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("Missing asset " + gameObject.name);
        } else {
            instance = this;
        }
    }
 
    // Start is called before the first frame update
    void Start()
    {
        //Getting a random sequence of the statues in statuesOrder array:
        SetPuzzle();
    }

    public void SetPuzzle()
    {
        nSelected = 0;
        int count = 4, random;        
        List<Id> statues = new List<Id>();
        for (int i = 0; i < 4; i++) {
            statues.Add((Id)i);
        }
        while (count > 0) {
            random = Random.Range(0, count);
            statuesCorrectOrder[4 - count] = statues[random];
            statues.RemoveAt(random);
            count--;
        }
        Debug.Log("The correct order is: " + statuesCorrectOrder[0] + ", " + statuesCorrectOrder[1] + ", " + statuesCorrectOrder[2] + " & " + statuesCorrectOrder[3] + ".");
    }

    public void SelectStatue(Id id)
    {
        statuesSelectedOrder[nSelected++] = id;
        Debug.Log("The statue number "+nSelected+" selected is the "+id+" one!");
        /*if(nSelected == 4)
        {
            Debug.Log(CheckFinalAnswer());
        }*/
    }

    public bool CheckFinalAnswer()
    {
        if (nSelected < 4) return false;

        for(int i=0; i<4; i++)
        {
            if(statuesCorrectOrder[i] != statuesSelectedOrder[i]) 
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
