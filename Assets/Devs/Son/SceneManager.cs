using UnityEngine.SceneManagement;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{

    public static MySceneManager Instance;

    private int lastMap = 3;

    private void Awake()
    {

        if (
            Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    // Load the Main Menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Load Map 1 scene
    public void LoadMap1()
    {
        SceneManager.LoadScene(1);
        lastMap = 1;
    }

    // Load Map 2 scene
    public void LoadMap2()
    {
        SceneManager.LoadScene(2);
        lastMap = 2;
    }

    public void LoadMap3()
    {
        SceneManager.LoadScene(5);
        lastMap = 5;
    }

    // Load the Map Select scene
    public void LoadMapSelect()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadPreviousMap()
    {
        SceneManager.LoadScene(lastMap);
    }

    // Load the End Screen scene
    public void LoadEndScreen()
    {
        SceneManager.LoadScene(4);
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Game is quitting...");
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
