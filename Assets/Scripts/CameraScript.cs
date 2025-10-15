using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// G�re le comportement de la cam�ra qui suit le joueur et permet sa rotation avec la souris.
/// </summary>
public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform playerBody;        // R�f�rence au Transform du joueur � suivre
    [SerializeField] private float sensitivity = 100f;    // Sensibilit� de la rotation de la cam�ra
    [SerializeField] private float clampAngle = 80f;     // Angle maximum de rotation verticale
    [SerializeField] private float distanceFromPlayer = 3f; // Distance entre la cam�ra et le joueur
    [SerializeField] private float height = 1.6f;        // Hauteur de la cam�ra par rapport au joueur

    private CameraControls controls;                      // Syst�me de contr�les de la cam�ra
    private Vector2 lookInput;                           // Valeur d'entr�e de la souris
    private float xRotation = 0f;                        // Rotation verticale actuelle
    private float yRotation = 0f;                        // Rotation horizontale actuelle
    //private bool isFirstPerson = false;          // Indique si la cam�ra est en vue � la premi�re personne

    /// <summary>
    /// Initialise les contr�les de la cam�ra et configure les callbacks d'entr�e.
    /// </summary>
    private void Awake()
    {
        controls = new CameraControls();
        // Configure la d�tection du mouvement de la souris
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    // Active les contr�les de la cam�ra
    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    // D�sactive les contr�les de la cam�ra
    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    /// <summary>
    /// Initialise la rotation de la cam�ra en fonction de sa position de d�part par rapport au joueur.
    /// </summary>
    private void Start()
    {
        Vector3 offset = transform.position - playerBody.position;
        // Calcule les angles de rotation initiaux � partir de la position relative
        yRotation = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        xRotation = Mathf.Asin(offset.y / offset.magnitude) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Met � jour la position et la rotation de la cam�ra � chaque frame apr�s les mises � jour du joueur.
    /// </summary>
    private void LateUpdate()
    {
        if (controls.Camera.ToggleView.triggered) Debug.Log("Toggle View - First Person");
        else Debug.Log("Toggle View - Third Person");

        // Calcule la nouvelle position de la cam�ra pour suivre le joueur
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer);
        transform.position = playerBody.position + Vector3.up * height + offset;

        // Fait pointer la cam�ra vers le joueur
        transform.LookAt(playerBody.position + Vector3.up * height);

        // G�re la rotation de la cam�ra uniquement lorsque le bouton gauche de la souris est maintenu
        if (Mouse.current.leftButton.isPressed)
        {
            // Calcule les rotations en fonction du mouvement de la souris
            float mouseX = lookInput.x * sensitivity * Time.deltaTime;
            float mouseY = lookInput.y * sensitivity * Time.deltaTime;

            // Met � jour les angles de rotation
            yRotation += mouseX;
            xRotation -= mouseY;
            // Limite la rotation verticale pour �viter le retournement de la cam�ra
            xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
        }
    }
}
