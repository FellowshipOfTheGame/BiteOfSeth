using UnityEngine.UI;
using UnityEngine;
using System;

public class HUDController : MonoBehaviour
{
    public Text scoreText;
    public Text statuesText;
    public Text piecesOfLoreText;
    public Text timerText;

    private PuzzleManager pm;
    private string totalStatues;
    private string totalScore;

    private void Start()
    {
        totalScore = ServiceLocator.Get<GameManager>().GetLevelDiamondsTotal().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        string score = ServiceLocator.Get<GameManager>().GetLevelScore().ToString();
        
        scoreText.text = score + "/" + totalScore;

        pm = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager();
        //totalStatues = pm.GetStatuesQuantity().ToString();
        totalStatues = pm.GetTotalStatuesQuantity().ToString(); 
        //string currentStatues = pm.GetSelectedStatuesQuantity().ToString();
        string currentStatues = pm.GetTipStatuesQuantity().ToString();
        statuesText.text = currentStatues + "/" + totalStatues;

        string piecesOfLore = ServiceLocator.Get<GameManager>().GetLevelPiecesOfLore().ToString();
        string totalPieces = ServiceLocator.Get<GameManager>().GetLevelLoreTotal().ToString();
        piecesOfLoreText.text = piecesOfLore + "/5";

        var ts = TimeSpan.FromSeconds(ServiceLocator.Get<GameManager>().GetLevelTimer());
        //Debug.Log(ServiceLocator.Get<GameManager>().GetLevelTimer());
        timerText.text = string.Format("{0:00}:{1:00}", (int)ts.TotalMinutes, ts.Seconds);

    }

   

}
