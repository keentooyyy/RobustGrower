using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    public string gameSceneName = "Level1"; // 👈 Set the exact scene name you want to load

    public void PlayGame()
    {
        Debug.Log("Loading game scene: " + gameSceneName);
        Time.timeScale = 1f; // ✅ Ensure normal time before loading
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Editor
#else
        Application.Quit(); // Quits in a built game
#endif
    }
}
