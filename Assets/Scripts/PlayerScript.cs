using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;           // Vitesse de déplacement du joueur
    [SerializeField] float rotationSpeed = 120f;     // Vitesse de rotation du joueur
    [SerializeField] Transform playerStartPoint;     // Point de départ du joueur

    private PlayerControls controls;                  // Référence au système d'input personnalisé
    private Vector2 moveInput;                        // Stocke l'entrée de déplacement (x = rotation, y = déplacement avant/arrière)
    private Rigidbody rb;                             // Référence au Rigidbody du joueur
    Animator animator;

    /// <summary>
    /// Initialise les contrôles et configure les callbacks d'entrée.
    /// </summary>
    void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();


        // Callback appelé quand le joueur bouge (input maintenu)
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        // Callback appelé quand le joueur arrête de bouger (input relâché)
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    /// <summary>
    /// Active les contrôles du joueur.
    /// </summary>
    void OnEnable()
    {
        if (controls != null)
            controls.Player.Enable();
    }

    /// <summary>
    /// Désactive les contrôles du joueur.
    /// </summary>
    void OnDisable()
    {
        if (controls != null)
            controls.Player.Disable();
    }

    /// <summary>
    /// Initialisation du Rigidbody et configuration des contraintes physiques.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Le Player doit avoir un Rigidbody !");

        // Empêche le joueur de basculer sur les axes X et Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Améliore la fluidité des mouvements physiques
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Vérifie que le point de départ est assigné
        if (playerStartPoint == null)
            Debug.LogWarning("ATTENTION : Le point de départ (playerStartPoint) n'est pas assigné !");

        // Place le joueur au point de départ
        transform.position = playerStartPoint.position;
    }

    /// <summary>
    /// Appelé à chaque frame physique, gère le déplacement et la rotation du joueur.
    /// </summary>
    void FixedUpdate()
    {
        MoveAndRotate();
    }

    /// <summary>
    /// Déplace et fait tourner le joueur selon les entrées utilisateur.
    /// </summary>
        private void MoveAndRotate()
    {
        // Vérifie si le joueur se déplace réellement
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("Move", isMoving);

        // Déplacement avant/arrière (axe Y de moveInput)
        Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotation gauche/droite (axe X de moveInput)
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
