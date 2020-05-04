using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    private int[] statuesCorrectOrder = new int[4];
    private int[] statuesSelectedOrder = new int[4];
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
        int count = 4, random;
        List<int> statues = new List<int>();
        for(int i=0; i<4; i++)
        {
            statues.Add(i);
        }
        while (count > 0) 
        {
            random = Random.Range(0, count);
            statuesCorrectOrder[4 - count] = statues[random];
            statues.RemoveAt(random);
            count--;
        }

        for (int i = 0; i < 4; i++) {
            Debug.Log(statuesCorrectOrder[i]);
        }

    }

    public void SelectStatue(int id)
    {
        statuesSelectedOrder[nSelected++] = id;
        Debug.Log("A "+nSelected+"ª estátua selecionada é a estátua "+id+".");
        if(nSelected == 4)
        {
            Debug.Log(CheckFinalAnswer());
           /* if (CheckFinalAnswer()) 
            {

            }*/
        }
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
