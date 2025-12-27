using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyScript : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] float moveSpeed = 5f;               // Vitesse de déplacement
    [SerializeField] float detectionDistance = 1.5f;     // Distance du laser
    [SerializeField] Transform playerStartPoint;         // Point de respawn
    [SerializeField] LayerMask obstacleLayer;            // Coche "Wall" ici

    private Rigidbody rb;
    private Vector3 currentDirection;
    private float directionCooldown = 0f;

    /// <summary>
    /// Initialisation.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("Manque le Rigidbody !");

        // Important : On bloque la physique pour éviter que ça tremble
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        ChangeDirection();
    }

    /// <summary>
    /// Boucle physique.
    /// </summary>
    void FixedUpdate()
    {
        // 1. DESSIN DU RAYON (Visible dans l'onglet SCENE si Gizmos activés)
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Debug.DrawRay(rayOrigin, currentDirection * detectionDistance, Color.red);

        // 2. DETECTEUR LASER (Anticipation)
        if (Physics.Raycast(rayOrigin, currentDirection, detectionDistance, obstacleLayer))
        {
            if (Time.time > directionCooldown)
            {
                ChangeDirection();
                directionCooldown = Time.time + 0.5f;
            }
        }

        // 3. DEPLACEMENT
        Vector3 newPosition = rb.position + (currentDirection * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        // Orientation
        if (currentDirection != Vector3.zero) transform.forward = currentDirection;
    }

    /// <summary>
    /// Sécurité 1 : Choc frontal (Si le laser a raté).
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // Si on tape un mur
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ChangeDirection();
        }

        // Si on tape le joueur
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HandlePlayerCollision(collision);
        }
    }

    /// <summary>
    /// Sécurité 2 : Si on reste collé (Le "Débloqueur").
    /// </summary>
    private void OnCollisionStay(Collision collision)
    {
        // Si on est contre un mur ET qu'on n'avance plus (vitesse nulle)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Si la vitesse est très faible (donc bloqué), on force le changement
            if (rb.velocity.magnitude < 0.1f && Time.time > directionCooldown)
            {
                ChangeDirection();
                directionCooldown = Time.time + 0.5f;
            }
        }
    }

    /// <summary>
    /// Gestion propre de la collision joueur.
    /// </summary>
    private void HandlePlayerCollision(Collision collision)
    {
        Debug.Log("Joueur touché !");
        if (playerStartPoint != null)
            collision.transform.position = playerStartPoint.position;

        Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            playerRb.rotation = Quaternion.identity;
        }
    }

    /// <summary>
    /// Change la direction aléatoirement.
    /// </summary>
    private void ChangeDirection()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

        // Petite logique pour ne pas reprendre la même direction
        Vector3 oldDirection = currentDirection;
        int attempts = 0;

        do
        {
            currentDirection = directions[Random.Range(0, directions.Length)];
            attempts++;
        }
        while (currentDirection == oldDirection && attempts < 10);
    }
}