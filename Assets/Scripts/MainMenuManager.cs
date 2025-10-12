using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void StartGame()
    {
        Debug.Log("Start Game");
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        // Quit the application
        Application.Quit();
    }
}
