using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private GameObject[] puzzleStatuesReferences;

    //Quantity of statues in the puzzle
    private int statuesQuantity;

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
                names[i] = pod.name;
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
                Debug.Log(j + 1 + "ª: " + (int)statuesCorrectOrder[j]);
            }
        }
    }

    public void SelectStatue(Id id)
    {
        statuesSelectedOrder[nSelected++] = id;
        Debug.Log("The statue number "+nSelected+" selected is the "+(int)id+" one!");
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

}
