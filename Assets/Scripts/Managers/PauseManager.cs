using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public AudioSource musicSource; // 🎵 Drag your background music AudioSource here

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        isPaused = true;

        if (musicSource != null && musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        isPaused = false;

        if (musicSource != null && !musicSource.isPlaying)
            musicSource.UnPause(); // resumes from where it paused
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
