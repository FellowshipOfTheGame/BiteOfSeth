using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NextLevelTrigger : MonoBehaviour
{
    public SceneReference NextLevelScene;
    public Text levelDiamondsLabel;
    public Text totalDiamondsLabel;
    public Text levelLoreLabel;
    public Text totalLoreLabel;
    public Text timeLabel;
    public Text totalTimeLabel;
    public GameObject EndLevelMenu;
    public GameObject button;
    public GameObject diamondImage;

    private int levelCollectedDiamonds;
    private int levelTotalDiamonds;
    private int totalCollectedDiamonds;
    public int totalGameDiamonds = 200;

    private int levelCollectedLore;
    private int levelTotalLore;
    private int totalCollectedLore;
    public int totalGameLore = 10;

    public bool showCongratsScreen = true;

    private GameManager gm;

    public void Start()
    {
        gm = ServiceLocator.Get<GameManager>();
        if (showCongratsScreen) {
            levelTotalDiamonds = gm.GetLevelDiamondsTotal();
            Debug.Log("Level tem " + levelTotalDiamonds + " diamantes, no total.");
            levelTotalLore = gm.GetLevelLoreTotal();
            Debug.Log("Level tem " + levelTotalLore + " pieces of lore, no total.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            ServiceLocator.Get<GameManager>().StopTimer();
            ServiceLocator.Get<GameManager>().lockMovement++;
            if (NextLevelScene != null) {
                collision.gameObject.GetComponent<Movable>().StopSfx();
                collision.gameObject.GetComponent<Movable>().enabled = false;
                if (showCongratsScreen) {

                    levelCollectedDiamonds = gm.GetLevelScore();
                    totalCollectedDiamonds = gm.GetTotalScore() + levelCollectedDiamonds;
                    levelDiamondsLabel.text = levelCollectedDiamonds.ToString() + " / " + levelTotalDiamonds.ToString();
                    totalDiamondsLabel.text = totalCollectedDiamonds.ToString();

                    levelCollectedLore = gm.GetLevelPiecesOfLore();
                    totalCollectedLore = gm.GetTotalPiecesOfLore() + levelCollectedLore;
                    levelLoreLabel.text = levelCollectedLore.ToString() + " / " + levelTotalLore.ToString();
                    totalLoreLabel.text = totalCollectedLore.ToString();

                    var ts = TimeSpan.FromSeconds(gm.GetLevelTimer());
                    timeLabel.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds) + " min";
                    ts = TimeSpan.FromSeconds(gm.GetTotalTimer() + gm.GetLevelTimer());
                    totalTimeLabel.text = string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds) + " min";

                    diamondImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

                    EndLevelMenu.SetActive(true);
                    Selectable s = button.GetComponent<Selectable>();
                    s.Select();
                } else{
                    GoToNextLevel();
                }
            } else {
                Debug.LogError("Sem referência para a próxima cena.");
            }
        }
    }

    public void GoToNextLevel()
    {
        ServiceLocator.Get<GameManager>().lockMovement--;
        gm.FromLevelGoToScene(NextLevelScene);
    }

}
