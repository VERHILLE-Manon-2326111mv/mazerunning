using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère les actions du menu principal, telles que démarrer le jeu ou quitter l'application.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Nom de la scène du jeu principal")]
    [SerializeField] private string gameSceneName;

    /// <summary>
    /// Lance le jeu en chargeant la scène principale.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Start Game");
        // Charge la scène du jeu principal
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Quitte l'application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        // Ferme l'application
        Application.Quit();
    }
}