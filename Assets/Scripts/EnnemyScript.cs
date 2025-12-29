using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // INDISPENSABLE pour le NavMesh

public class EnnemyScript : MonoBehaviour
{
    [Header("Réglages Navigation")]
    [Tooltip("Rayon de recherche pour la destination (Mets 50 ou 100 pour couvrir tout le labyrinthe)")]
    [SerializeField] float range = 50f;                   // Rayon de recherche pour la destination aléatoire

    [Header("Réglages Interaction")]
    [SerializeField] Transform playerStartPoint;          // Point de réapparition du joueur après collision

    private NavMeshAgent agent;                           // Référence au composant de navigation

    /// <summary>
    /// Initialisation de l'agent et configuration physique.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // On s'assure que la physique ne perturbe pas le NavMesh
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // On lance la première destination dès le début
        SetNewRandomDestination();
    }

    /// <summary>
    /// Vérifie à chaque frame si l'agent a atteint sa destination.
    /// </summary>
    void Update()
    {
        // On vérifie si l'agent a fini de calculer (!pathPending) ET s'il est arrivé (remainingDistance <= stoppingDistance)
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Il est arrivé, on lui donne une nouvelle destination
            SetNewRandomDestination();
        }
    }

    /// <summary>
    /// Trouve un point valide sur la carte bleue (NavMesh) et y va.
    /// </summary>
    void SetNewRandomDestination()
    {
        // On cherche un point au hasard dans une sphère autour de l'ennemi
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;

        NavMeshHit hit;

        // On vérifie si ce point tombe bien sur la zone bleue (Walkable)
        // SamplePosition cherche le point le plus proche sur le NavMesh dans un rayon de 10 unités
        if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    /// <summary>
    /// Gestion de la collision avec le joueur.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // Si on tape le joueur
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Joueur touché !");

            // Téléportation au point de départ
            if (playerStartPoint != null)
                collision.transform.position = playerStartPoint.position;

            // Réinitialisation de la physique du joueur (pour stopper l'inertie)
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
        }
    }
}