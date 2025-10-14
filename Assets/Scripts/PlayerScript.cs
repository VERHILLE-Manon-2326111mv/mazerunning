using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotationSpeed = 120f; 

    private PlayerControls controls;
    private Vector2 moveInput;
    private Rigidbody rb;

    void Awake()
    {
        controls = new PlayerControls();

        // Quand le joueur bouge
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError("Le Player doit avoir un Rigidbody !");

        // Verrouiller les rotations X et Z pour ne pas qu'il tombe sur le côté
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        // Déplacement du joueur avec Rigidbody (gère les collisions)
        // Pour Z/S | W/S | Haut/Bas
        Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Pour Q/D | Q/D | Gauche/Droite
        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}
