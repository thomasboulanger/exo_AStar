﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject GameOverPanel;
    public GameObject InGamePanel;
    public Text scoreText;
    public Text InGameScoreText;

    private GameController gameController;
    private AudioController audioController;


    private void Awake()
    {
        var gc = GameObject.FindWithTag("GameController");
        var ac = GameObject.FindWithTag("AudioController");
        if (null == gc)
        {
            Debug.LogError("[UIController] GameController missing");
        }
        else
        {
            gameController = gc.GetComponent<GameController>();
        }
        if (null == ac)
        {
            Debug.LogError("[UIController] AudioController missing");
        }
        else
        {
            audioController = ac.GetComponent<AudioController>();
        }
    }

    void Start()
    {
        scoreText.text = "";
        InGameScoreText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            audioController.ButtonClic();
            if (gameController.isPaused)
            {
                ResumeMenu();
            }
            else
            {
                PauseMenu();
                gameController.isPaused = true;
            }
        }
        scoreText.text = "" + PlayerController.score.ToString();
        InGameScoreText.text = "" + PlayerController.score.ToString();

        if (gameController.gameOver == true && !GameOverPanel.activeSelf)
        {
            GameOver();
        }
    }

    public void PauseMenu()
    {
        PauseMenuUI.SetActive(true);
        gameController.isPaused = true;
    }
    public void ResumeMenu()
    {
        PauseMenuUI.SetActive(false);
        gameController.isPaused = false;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Game_Scene");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Intro_Scene");
    }
    public void GameOver()
    {
        gameController.isPaused = true;
        PauseMenuUI.SetActive(false);
        InGamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
}
