using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader Instance;  // Singleton instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this GameObject across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    public void LoadNewScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void UnloadCurrentScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
}