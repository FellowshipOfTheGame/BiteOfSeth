﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/PuzzleManager")]
public class PuzzleManager : GameService
{
    private GameObject[] puzzleStatuesReferences;

    //Quantity of statues in the puzzle
    private int statuesQuantity;

    //if, in the future, we will need more ids, just add more
    public static int maxColorsQuantity = 20;
    public enum Id { Red = 0, Blue = 1, Green = 2, Yellow = 3,  White = 4, Black = 5, Orange = 6, Purple = 7, Gray = 8, Pink = 9}; //...

    private Id[] statuesCorrectOrder = new Id[maxColorsQuantity];
    private Id[] statuesSelectedOrder = new Id[maxColorsQuantity];
    private int nSelected = 0;
 
    // Start is called before the first frame update
    public override void Start()
    {
        //Get all puzzle statues references in scene
        puzzleStatuesReferences = GameObject.FindGameObjectsWithTag("PuzzleStatue");
        statuesQuantity = puzzleStatuesReferences.Length;
        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        foreach(GameObject S in puzzleStatuesReferences) {
            PuzzleOrderDialogue pod = S.GetComponent<PuzzleOrderDialogue>();
            if(pod != null) {
                pod.ResetSelection();
            }
        }
        //Getting a random sequence of the statues in statuesOrder array:
        nSelected = 0;
        int count = statuesQuantity, random;        
        List<Id> statues = new List<Id>();
        for (int i = 0; i < statuesQuantity; i++) {
            statues.Add((Id)i);
        }
        while (count > 0) {
            random = Random.Range(0, count);
            statuesCorrectOrder[statuesQuantity - count] = statues[random];
            statues.RemoveAt(random);
            count--;
        }
        Debug.Log("The correct order is: ");
        for (int i=0; i<statuesQuantity; i++) {
            Debug.Log(i+1 + "ª: " + statuesCorrectOrder[i]);
        }
    }

    public void SelectStatue(Id id)
    {
        statuesSelectedOrder[nSelected++] = id;
        Debug.Log("The statue number "+nSelected+" selected is the "+id+" one!");
        /*if(nSelected == statuesQuantity)
        {
            Debug.Log(CheckFinalAnswer());
        }*/
    }

    public bool CheckFinalAnswer()
    {
        if (nSelected < statuesQuantity) return false;

        for(int i=0; i<statuesQuantity; i++)
        {
            if(statuesCorrectOrder[i] != statuesSelectedOrder[i]) 
            {
                return false;
            }
        }

        return true;
    }

}
