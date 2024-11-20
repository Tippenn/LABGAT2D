using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text scoreText;

    public LevelManager levelManager;
    public AudioManager audioManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
        audioManager = AudioManager.Instance;  
    }
    private void OnEnable()
    {
        CalculateHighScore();
        CalculateScore();
    }

    public void CalculateHighScore()
    {
        LevelManager.SaveObject highScore = SaveSystem.LoadMostRecentObject<LevelManager.SaveObject>();
        highScoreText.text = "Highscore: " + highScore.score.ToString();
    }

    public void CalculateScore()
    {
        Debug.Log(levelManager.score);
        scoreText.text = "Score: " + levelManager.score.ToString();
    }

    public void RestartClicked()
    {
        SceneManager.LoadScene("TestingScene");
    }

    public void MainMenuClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
