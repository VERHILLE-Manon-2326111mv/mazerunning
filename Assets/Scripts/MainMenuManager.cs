using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re les actions du menu principal, telles que d�marrer le jeu ou quitter l'application.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName; // Nom de la sc�ne principale � charger lors du d�marrage du jeu

    /// <summary>
    /// Lance le jeu en chargeant la sc�ne principale.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Start Game");
        // Charge la sc�ne du jeu principal
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