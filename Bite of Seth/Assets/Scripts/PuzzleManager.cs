﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private GameObject[] puzzleStatuesReferences;

    //Quantity of statues in the puzzle
    private int statuesQuantity;
    private int tipStatuesQuantity;

    //if, in the future, we will need more ids, just add more
    public static int maxIdsQuantity = 20;
    
    public enum Id { Red = 0, Blue = 1, Green = 2, Yellow = 3,  White = 4, Black = 5, Orange = 6, Purple = 7, Gray = 8, Pink = 9}; //...

    private Id[] statuesCorrectOrder = new Id[maxIdsQuantity];
    private Id[] statuesSelectedOrder = new Id[maxIdsQuantity];
    private string[] names = new string[maxIdsQuantity];

    private int nSelected = 0;
 
    // Start is called before the first frame update
    public void Start()
    {
        //Get all puzzle statues references in scene
        puzzleStatuesReferences = GameObject.FindGameObjectsWithTag("PuzzleStatue");
        statuesQuantity = puzzleStatuesReferences.Length;
        ResetPuzzle();
        tipStatuesQuantity = 0;
    }

    public void ResetPuzzle()
    {
        List<Id> statues = new List<Id>();
        int i = 0;
        foreach (GameObject S in puzzleStatuesReferences) {
            PuzzleOrderDialogue pod = S.GetComponent<PuzzleOrderDialogue>();
            if(pod != null) {
                statues.Add((Id)i);
                pod.SetId((Id)i);
                pod.ResetSelection();
                names[i] = pod.statueName;
                i++;
            }
        }
        //Getting a random sequence of the statues in statuesOrder array:
        nSelected = 0;
        int count = statuesQuantity, random;        

        while (count > 0) {
            random = Random.Range(0, count);
            statuesCorrectOrder[statuesQuantity - count] = statues[random];
            statues.RemoveAt(random);
            count--;
        }
        if (statuesQuantity > 0) 
        {
            Debug.Log("The correct order is: ");
            for (int j = 0; j < statuesQuantity; j++) {
                Debug.Log(j + 1 + "ª: " + names[(int)statuesCorrectOrder[j]]);
            }
        }
    }

    public int SelectStatue(Id id)
    {
        statuesSelectedOrder[nSelected++] = id;
        Debug.Log("Selection number "+nSelected+": statue "+(int)id+" named "+names[(int)id] +"!");
        /*if(nSelected == statuesQuantity)
        {
            Debug.Log(CheckFinalAnswer());
        }*/

        return nSelected;
    }

    public void UnselectStatue(Id id){
        bool found = false;

        for (int i = 0; i < nSelected; i++) {
            if (found) {
                Id aux = statuesSelectedOrder[i];
                statuesSelectedOrder[i - 1] = aux;
                puzzleStatuesReferences[(int) aux].GetComponent<PuzzleOrderDialogue>().UpdateCounter(i);
            }

            if (statuesSelectedOrder[i] == id) found = true;
        }

        if(found) nSelected--;
    }

    public int CheckFinalAnswer()
    {
        //Não selecionou o suficiente
        if (nSelected < statuesQuantity) return 0;

        for(int i=0; i<statuesQuantity; i++)
        {
            //A escolha foi na ordem errada
            if(statuesCorrectOrder[i] != statuesSelectedOrder[i]) 
            {
                return 2;
            }
        }

        //A escolha foi na ordem correta
        return 1;
    }

    public bool AllStatuesSelected()
    {
        return (nSelected == statuesQuantity);
    }


    public string[] GetStatuesNamesInOrder()
    {
        string[] namesInOrder = new string[statuesQuantity];
        if(statuesQuantity > 0) {
            for (int i=0; i<statuesQuantity; i++) {
                namesInOrder[i] = names[(int)statuesCorrectOrder[i]];
            }
        }
        return namesInOrder;
    }


    public void ResetChoices()
    {
        foreach (GameObject S in puzzleStatuesReferences) {
            PuzzleOrderDialogue pod = S.GetComponent<PuzzleOrderDialogue>();
            if (pod != null) {
                pod.ResetSelection();
            }
        }
        
        nSelected = 0;

    }

    public int GetStatuesQuantity()
    {
        return statuesQuantity;
    }

    public int GetSelectedStatuesQuantity()
    {
        return nSelected;
    }

    public void AddTipStatue()
    {
        tipStatuesQuantity++;
    }

    public int GetTipStatuesQuantity()
    {
        return tipStatuesQuantity;
    }

}
