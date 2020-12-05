using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private GameObject[] puzzleStatuesReferences;
    private GameObject finalpuzzleStatueRef;

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

    private int totalStatuesQuantity;

    // Start is called before the first frame update
    public void Start()
    {
        //Get all puzzle statues references in scene
        puzzleStatuesReferences = GameObject.FindGameObjectsWithTag("PuzzleStatue");
        totalStatuesQuantity = puzzleStatuesReferences.Length;

        finalpuzzleStatueRef = GameObject.FindGameObjectWithTag("FinalStatue");
        PuzzleFinalDialogue pfod = finalpuzzleStatueRef.GetComponent<PuzzleFinalDialogue>();       

        if (pfod.IsAllStatuesSelectable()) {
            statuesQuantity = totalStatuesQuantity;
        } else {
            statuesQuantity = pfod.GetPuzzleTotalSelectionsQuantity();
        }

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
        //int count = statuesQuantity;
        int count = totalStatuesQuantity;
        int random;

        while (count > 0) {
            random = Random.Range(0, count);
            statuesCorrectOrder[totalStatuesQuantity - count] = statues[random];
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

        if (nSelected >= statuesQuantity-1) {
            Id aux = statuesSelectedOrder[statuesQuantity - 1];
            for (int i = statuesQuantity - 1; i >= 1; i--) {
                statuesSelectedOrder[i] = statuesSelectedOrder[i - 1];
                puzzleStatuesReferences[(int)statuesSelectedOrder[i]].GetComponent<PuzzleOrderDialogue>().UpdateCounter(i);
            }
            statuesSelectedOrder[0] = id;
            puzzleStatuesReferences[(int)aux].GetComponent<PuzzleOrderDialogue>().ResetSelection();
        } else {
            statuesSelectedOrder[nSelected++] = id;
        }
        Debug.Log("Selection number "+nSelected+": statue "+(int)id+" named "+names[(int)id] +"!");

        /*if(nSelected == statuesQuantity){
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
        string[] namesInOrder = new string[totalStatuesQuantity];
        if(totalStatuesQuantity > 0) {
            for (int i=0; i< totalStatuesQuantity; i++) {
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

    public void LockStatues() {
        foreach (GameObject S in puzzleStatuesReferences) {
            PuzzleOrderDialogue pod = S.GetComponent<PuzzleOrderDialogue>();
            if (pod != null) {
                pod.SetLock(true);
            }
        }
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
