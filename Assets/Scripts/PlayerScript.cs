using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Param�tre des mouvements du joueur")]
    [SerializeField] float moveSpeed = 2f;           // Vitesse de d�placement du joueur
    [SerializeField] float rotationSpeed = 120f;     // Vitesse de rotation du joueur
    [SerializeField] Transform playerStartPoint;     // Point de d�part du joueur

    private PlayerControls controls;                  // R�f�rence au syst�me d'input personnalis�
    private Vector2 moveInput;                        // Stocke l'entr�e de d�placement (x = rotation, y = d�placement avant/arri�re)
    private Rigidbody rb;                             // R�f�rence au Rigidbody du joueur
    public bool canMove = true;                       // Indique si le joueur peut se deplacer
    Animator animator;                                // Reference � l'Animator du joueur

    /// <summary>
    /// Initialise les contr�les et configure les callbacks d'entr�e.
    /// </summary>
    void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();


        // Callback appel� quand le joueur bouge (input maintenu)
        controls.Player.Move.performed += ctx => {
            moveInput = ctx.ReadValue<Vector2>();
        };
        // Callback appel� quand le joueur arr�te de bouger (input rel�ch�)
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    /// <summary>
    /// Active les contr�les du joueur.
    /// </summary>
    void OnEnable()
    {
        if (controls != null)
            controls.Player.Enable();
    }

    /// <summary>
    /// D�sactive les contr�les du joueur.
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

        // Emp�che le joueur de basculer sur les axes X et Z
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Am�liore la fluidit� des mouvements physiques
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // V�rifie que le point de d�part est assign�
        if (playerStartPoint == null)
            Debug.LogWarning("ATTENTION : Le point de d�part (playerStartPoint) n'est pas assign� !");

        // Place le joueur au point de d�part
        transform.position = playerStartPoint.position;
    }

    /// <summary>
    /// Appel� � chaque frame physique, g�re le d�placement et la rotation du joueur.
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
    /// D�place et fait tourner le joueur selon les entr�es utilisateur.
    /// </summary>
        private void MoveAndRotate()
    {
        // V�rifie si le joueur se d�place r�ellement
        bool isMoving = moveInput.magnitude > 0.1f;
        animator.SetBool("Move", isMoving);

        // D�placement avant/arri�re (axe Y de moveInput)
        Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotation gauche/droite (axe X de moveInput)
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }

    /// <summary>
    /// Empeche ou autorise le d�placement du joueur
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
