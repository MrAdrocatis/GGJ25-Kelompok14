using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    [Header("Custom Scene Names")]
    public string customScene1; // Scene custom 1
    public string customScene2; // Scene custom 2
    public string customScene3; // Scene custom 3

    // Exit Application
    public void ExitApplication()
    {
        Debug.Log("Exiting application...");
        Application.Quit();
    }

    // Restart Current Scene
    public void RestartScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Restarting scene: {currentSceneName}");
        SceneManager.LoadScene(currentSceneName);
    }

    // Load Custom Scene 1
    public void LoadCustomScene1()
    {
        LoadSceneByName(customScene1);
    }

    // Load Custom Scene 2
    public void LoadCustomScene2()
    {
        LoadSceneByName(customScene2);
    }

    // Load Custom Scene 3
    public void LoadCustomScene3()
    {
        LoadSceneByName(customScene3);
    }

    // Helper function to load a scene by name
    private void LoadSceneByName(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log($"Loading scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' does not exist or is not added to the build settings.");
        }
    }
}
