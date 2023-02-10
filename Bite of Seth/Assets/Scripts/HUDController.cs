using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class HUDController : MonoBehaviour
{
    public Text scoreText;
    public Text statuesText;
    public Text piecesOfLoreText;
    public Text timerText;
    public Text levelText;

    [Space(5)]
    public Animation EnigmaBlock;
    public Text EnigmaTitle;
    public Text EnigmaText;
    public Image EnigmaMark;
    public Text[] choosedStatues = new Text[5];

    private PuzzleManager pm;
    private bool enigmaStarted;
    private string totalStatues;
    private string totalScore;
    private int i;

    private void Start()
    {
        totalScore = ServiceLocator.Get<GameManager>().GetLevelDiamondsTotal().ToString();
        Scene scene = SceneManager.GetActiveScene();
        levelText.text = scene.name;
        enigmaStarted = false;
        EnigmaBlock.gameObject.SetActive(false);
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

        if (enigmaStarted) {
            string[] statues = pm.GetSelectedStatues();
            i = 0;
            while (i < statues.Length) {
                choosedStatues[i].text = (i + 1).ToString() + ". " + statues[i];
                choosedStatues[i].gameObject.SetActive(true);
                i++;
            }
            while (i < choosedStatues.Length) {
                choosedStatues[i].text = (i + 1).ToString() + ". _______";
                choosedStatues[i].gameObject.SetActive(false);
                i++;
            }
        }
    }

    public void OpenEnigma(DialogueBase question) {
        DialogueBase.Info enigma = question.dialogueInfo[question.dialogueInfo.Length - 1];
        
        EnigmaTitle.text = question.dialogueTitle;
        EnigmaText.text = enigma.myText;
        EnigmaMark.sprite = enigma.character.portrait;

        enigmaStarted = true;
        EnigmaBlock.gameObject.SetActive(true);
        EnigmaBlock.Play("enigma_open");
    }

    public void CloseEnigma() {
        EnigmaBlock.Play("enigma_close");
    }
}
