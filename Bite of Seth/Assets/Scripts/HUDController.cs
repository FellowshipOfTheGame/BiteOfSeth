using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour
{
    public Text scoreText;
    public Text statuesText;
    public Text piecesOfLoreText;

    private PuzzleManager pm;
    private string totalStatues;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string score = ServiceLocator.Get<GameManager>().GetLevelScore().ToString();
        scoreText.text = $"{score}";

        pm = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager();
        //totalStatues = pm.GetStatuesQuantity().ToString();
        totalStatues = pm.GetTotalStatuesQuantity().ToString(); 
        //string currentStatues = pm.GetSelectedStatuesQuantity().ToString();
        string currentStatues = pm.GetTipStatuesQuantity().ToString();
        statuesText.text = currentStatues + "/" + totalStatues;

        string piecesOfLore = ServiceLocator.Get<GameManager>().GetLevelPiecesOfLore().ToString();
        piecesOfLoreText.text = piecesOfLore + "/5";

    }

   

}
