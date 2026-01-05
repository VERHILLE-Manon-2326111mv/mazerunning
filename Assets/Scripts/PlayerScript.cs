using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Paramètre des mouvements du joueur")]
    [SerializeField] float moveSpeed = 2f;           // Vitesse de déplacement du joueur
    [SerializeField] float rotationSpeed = 120f;     // Vitesse de rotation du joueur

    private PlayerControls controls;                  // Référence au système d'input personnalisé
    private Vector2 moveInput;                        // Stocke l'entrée de déplacement (x = rotation, y = déplacement avant/arrière)
    private Rigidbody rb;                             // Référence au Rigidbody du joueur
    public bool canMove = true;                       // Indique si le joueur peut se deplacer
    Animator animator;                                // Reference à l'Animator du joueur

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
    }

    /// <summary>
    /// Appelé à chaque frame physique, gère le déplacement et la rotation du joueur.
    /// </summary>
    void FixedUpdate()
    {
        // Si le joueur ne peut pas se deplacer, arrete tout mouvement
        if (!canMove)
        {
            animator.SetBool("Move", false);
            rb.velocity = Vector3.zero;
            return;
        }

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

    /// <summary>
    /// Empeche ou autorise le déplacement du joueur
    /// </summary>
    public void SetCanMove(bool status)
    {
        canMove = status;

        if (!status)
        {
            moveInput = Vector2.zero;
            animator.SetBool("Move", false);
            rb.velocity = Vector3.zero;
        }
    }
}
