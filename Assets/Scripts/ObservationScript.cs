using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservationScript : MonoBehaviour
{
    [Header("Références des Caméras")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject observationCamera;

    [Header("Touche d'interaction avec la zone d'observation")]
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    // Etats des cameras
    private bool isPlayerOnPoint = false;
    private bool isObserving = false; 

    private PlayerScript playerScriptReference;

    // Initilisation des cameras
    private void Start()
    {
        playerCamera.SetActive(true);
        observationCamera.SetActive(false);
    }

    // Détecte l'entree du joueur dans la zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPoint = true;
            Debug.Log("Le joueur est entré dans la zone.");

            playerScriptReference = other.GetComponent<PlayerScript>();
        }
    }

    // Détecte la sortie du joueur de la zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPoint = false;
            Debug.Log("Le joueur a quitté la zone.");

            if (isObserving)
            {
                ToggleView();
            }

            playerScriptReference = null;
        }
    }

    // Verifie l'appui sur la touche à chaque frame
    private void Update()
    {
        if (isPlayerOnPoint && playerScriptReference != null && Input.GetKeyDown(interactionKey))
        {
            ToggleView();
        }
    }

    /// <summary>
    /// Inverse l'etat d'observation (change les cameras).
    /// </summary>
    private void ToggleView()
    {
        // Inverse l'état
        isObserving = !isObserving;

        // Active/Desactive les cameras en fonction du nouvel etat
        playerCamera.SetActive(!isObserving);
        observationCamera.SetActive(isObserving);

        if (playerScriptReference != null)
        {
            playerScriptReference.SetCanMove(!isObserving);
        }
    }
}