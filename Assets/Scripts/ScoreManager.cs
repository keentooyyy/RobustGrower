using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public PlayerMovement player;

    private float currentScore = 0f;
    private int highScore = 0;
    private bool isRunning = true;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    void Update()
    {
        if (!isRunning || player == null) return;

        currentScore += player.GetMoveSpeed() * Time.deltaTime;
        UpdateUI();
    }

    public void StopScore()
    {
        isRunning = false;
        int finalScore = Mathf.FloorToInt(currentScore);

        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "" + Mathf.FloorToInt(currentScore);

        if (highScoreText != null)
            highScoreText.text = "Best: " + highScore;
    }

    public int GetFinalScore()
    {
        return Mathf.FloorToInt(currentScore);
    }
}
