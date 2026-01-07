using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationScript : MonoBehaviour
{
    [Header("Références des Caméras")]
    [SerializeField] private GameObject playerCamera;           // Caméra principale du joueur (vue FPS/TPS)
    [SerializeField] private GameObject observationCamera;      // Caméra fixe du poste d'observation

    [Header("Touche d'interaction")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Touche pour entrer/sortir du mode observation

    // États internes
    private bool isPlayerOnPoint = false;                       // Le joueur est-il dans la zone de trigger ?
    private bool isObserving = false;                           // Le joueur est-il actuellement en train de regarder la caméra fixe ?

    private PlayerScript playerScriptReference;                 // Référence au script du joueur pour bloquer ses mouvements

    /// <summary>
    /// Initialisation de l'état des caméras au démarrage.
    /// </summary>
    private void Start()
    {
        // On s'assure que la vue commence sur le joueur
        playerCamera.SetActive(true);
        observationCamera.SetActive(false);
    }

    /// <summary>
    /// Vérifie l'appui sur la touche d'interaction à chaque frame.
    /// </summary>
    private void Update()
    {
        // Si le joueur est dans la zone, que le script est valide et qu'il appuie sur la touche
        if (isPlayerOnPoint && playerScriptReference != null && Input.GetKeyDown(interactionKey))
        {
            ToggleView();
        }
    }

    /// <summary>
    /// Détecte l'entrée du joueur dans la zone d'observation.
    /// </summary>
    /// <param name="other">Le collider qui entre dans la zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPoint = true;
            Debug.Log("Le joueur est entré dans la zone.");

            // Récupération du script pour pouvoir geler les mouvements plus tard
            playerScriptReference = other.GetComponent<PlayerScript>();
        }
    }

    /// <summary>
    /// Détecte la sortie du joueur de la zone et réinitialise la vue si nécessaire.
    /// </summary>
    /// <param name="other">Le collider qui sort de la zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPoint = false;
            Debug.Log("Le joueur a quitté la zone.");

            // Sécurité : Si le joueur part en laissant la caméra active, on coupe tout
            if (isObserving)
            {
                ToggleView();
            }

            playerScriptReference = null;
        }
    }

    /// <summary>
    /// Inverse l'état d'observation, change les caméras actives et gère le blocage du joueur.
    /// </summary>
    private void ToggleView()
    {
        // Inverse l'état
        isObserving = !isObserving;

        // Active/Désactive les caméras en fonction du nouvel état
        playerCamera.SetActive(!isObserving);
        observationCamera.SetActive(isObserving);

        // Bloque ou débloque les mouvements du joueur via son script
        if (playerScriptReference != null)
        {
            playerScriptReference.SetCanMove(!isObserving);
        }
    }
}