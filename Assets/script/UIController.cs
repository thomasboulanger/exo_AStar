using System.Collections;
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
    private void Awake()
    {
        var gc = GameObject.FindWithTag("GameController");
        if (null == gc)
        {
            Debug.LogError("[UIController] GameController missing");
        }
        else
        {
            gameController = gc.GetComponent<GameController>();
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

        if (gameController.gameOver == true)
        {
            gameController.isPaused = true;
            PauseMenuUI.SetActive(false);
            InGamePanel.SetActive(false);
            GameOverPanel.SetActive(true);
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
        SceneManager.LoadSceneAsync("Game_Scene");
        gameController.isPaused = false;
        gameController.gameOver = false;
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void ReturnToMainMenu()
    {
        gameController.gameOver = false;
        gameController.isPaused = false;
        SceneManager.LoadSceneAsync("Intro_Scene");
    }
}
