using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScoreDisplay : MonoBehaviour
{
    public TMP_Text highScoreText;

    public AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }
    private void OnEnable()
    {
        CalculateHighScore();
    }

    public void CalculateHighScore()
    {
        LevelManager.SaveObject highScore = SaveSystem.LoadMostRecentObject<LevelManager.SaveObject>();
        highScoreText.text = "Highscore: " + highScore.score.ToString();
    }

    public void MainMenuClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
