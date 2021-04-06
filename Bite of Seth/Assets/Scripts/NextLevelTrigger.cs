﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelTrigger : MonoBehaviour
{
    public SceneReference NextLevelScene;
    public Text levelDiamondsLabel;
    public Text totalDiamondsLabel;
    public GameObject EndLevelMenu;
    public GameObject button;

    private int levelCollectedDiamonds;
    private int levelTotalDiamonds;
    private int totalCollectedDiamonds;

    public void Start()
    {
        levelTotalDiamonds = ServiceLocator.Get<GameManager>().GetLevelDiamondsTotal();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            if (NextLevelScene != null) {

                levelCollectedDiamonds = ServiceLocator.Get<GameManager>().GetLevelScore();
                totalCollectedDiamonds = ServiceLocator.Get<GameManager>().GetTotalScore();
                levelDiamondsLabel.text = levelCollectedDiamonds.ToString() + " / " + levelTotalDiamonds.ToString();
                totalDiamondsLabel.text = totalCollectedDiamonds.ToString();
                EndLevelMenu.SetActive(true);
                Selectable s = button.GetComponent<Selectable>();
                s.Select();
                collision.gameObject.GetComponent<Movable>().StopSfx();
                collision.gameObject.GetComponent<Movable>().enabled = false;
            } else {
                Debug.LogError("Sem referência para a próxima cena.");
            }
        }
    }

    public void GoToNextLevel()
    {
        ServiceLocator.Get<GameManager>().FromLevelGoToScene(NextLevelScene);
    }

}
