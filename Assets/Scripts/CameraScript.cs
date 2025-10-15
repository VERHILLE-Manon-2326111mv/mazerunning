using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gère le comportement de la caméra qui suit le joueur et permet sa rotation avec la souris.
/// </summary>
public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform playerBody;        // Référence au Transform du joueur à suivre
    [SerializeField] private float sensitivity = 100f;    // Sensibilité de la rotation de la caméra
    [SerializeField] private float clampAngle = 80f;     // Angle maximum de rotation verticale
    [SerializeField] private float distanceFromPlayer = 3f; // Distance entre la caméra et le joueur
    [SerializeField] private float height = 1.6f;        // Hauteur de la caméra par rapport au joueur

    private CameraControls controls;                      // Système de contrôles de la caméra
    private Vector2 lookInput;                           // Valeur d'entrée de la souris
    private float xRotation = 0f;                        // Rotation verticale actuelle
    private float yRotation = 0f;                        // Rotation horizontale actuelle
    //private bool isFirstPerson = false;          // Indique si la caméra est en vue à la première personne

    /// <summary>
    /// Initialise les contrôles de la caméra et configure les callbacks d'entrée.
    /// </summary>
    private void Awake()
    {
        controls = new CameraControls();
        // Configure la détection du mouvement de la souris
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    // Active les contrôles de la caméra
    private void OnEnable()
    {
        controls.Camera.Enable();
    }

    // Désactive les contrôles de la caméra
    private void OnDisable()
    {
        controls.Camera.Disable();
    }

    /// <summary>
    /// Initialise la rotation de la caméra en fonction de sa position de départ par rapport au joueur.
    /// </summary>
    private void Start()
    {
        Vector3 offset = transform.position - playerBody.position;
        // Calcule les angles de rotation initiaux à partir de la position relative
        yRotation = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        xRotation = Mathf.Asin(offset.y / offset.magnitude) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Met à jour la position et la rotation de la caméra à chaque frame après les mises à jour du joueur.
    /// </summary>
    private void LateUpdate()
    {
        if (controls.Camera.ToggleView.triggered) Debug.Log("Toggle View - First Person");
        else Debug.Log("Toggle View - Third Person");

        // Calcule la nouvelle position de la caméra pour suivre le joueur
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer);
        transform.position = playerBody.position + Vector3.up * height + offset;

        // Fait pointer la caméra vers le joueur
        transform.LookAt(playerBody.position + Vector3.up * height);

        // Gère la rotation de la caméra uniquement lorsque le bouton gauche de la souris est maintenu
        if (Mouse.current.leftButton.isPressed)
        {
            // Calcule les rotations en fonction du mouvement de la souris
            float mouseX = lookInput.x * sensitivity * Time.deltaTime;
            float mouseY = lookInput.y * sensitivity * Time.deltaTime;

            // Met à jour les angles de rotation
            yRotation += mouseX;
            xRotation -= mouseY;
            // Limite la rotation verticale pour éviter le retournement de la caméra
            xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
        }
    }
}
