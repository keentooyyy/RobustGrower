using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public AudioClip gameOverSound;
    public AudioSource bgmSource; // 🔊 Assign your BGM AudioSource here

    private AudioSource audioSource;
    private bool isGameOver = false;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverSound != null && audioSource != null)
            audioSource.PlayOneShot(gameOverSound);

        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Pause(); // 🔇 Pause the main background music

        Debug.Log("Game Over triggered.");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game pressed.");
        Time.timeScale = 1f; // ✅ Reset before quitting

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
