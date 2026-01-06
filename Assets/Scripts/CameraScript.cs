using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Gere le comportement de la cam�ra qui suit le joueur et permet sa rotation avec la souris.
/// </summary>
public class CameraScript : MonoBehaviour
{
    [Header("Reference de l'objet a suivre")]
    [SerializeField] private Transform body;

    [Header("Parametres de la camera")]
    [SerializeField] private float sensitivity = 100f;      // Sensibilite de la rotation de la cam�ra
    [SerializeField] private float clampAngle = 80f;        // Angle maximum de rotation verticale
    [SerializeField] private float distanceFromObject = 3f; // Distance entre la camera et l'objet suivi
    [SerializeField] private float height = 1.6f;           // Hauteur de la camera par rapport a l'objet suivi

    private CameraControls controls;                        // Systeme de controles de la camera
    private Vector2 lookInput;                              // Valeur d'entree de la souris
    private float xRotation = 0f;                           // Rotation verticale actuelle
    private float yRotation = 0f;                           // Rotation horizontale actuelle
    private bool isFirstPerson = false;                     // Indique si la camera est en vue a la premiere personne (pour un joueur)

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
        if (controls != null)
            controls.Camera.Enable();
    }

    // D�sactive les contr�les de la cam�ra
    private void OnDisable()
    {
        if (controls != null)
            controls.Camera.Disable();
    }

    /// <summary>
    /// Initialise la rotation de la cam�ra en fonction de sa position de d�part par rapport au joueur.
    /// </summary>
    private void Start()
    {
        Vector3 offset = transform.position - body.position;
        // Calcule les angles de rotation initiaux � partir de la position relative
        yRotation = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        xRotation = Mathf.Asin(offset.y / offset.magnitude) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Met � jour la position et la rotation de la cam�ra � chaque frame apr�s les mises � jour du joueur.
    /// </summary>
    private void LateUpdate()
    {
        if (body == null) return;

        if (controls.Camera.ToggleView.triggered && body.CompareTag("Player")) changeView();

        // G�re la rotation de la cam�ra uniquement lorsque le bouton gauche de la souris est maintenu
        else if (Mouse.current.leftButton.isPressed)
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

        // Calcule la nouvelle position de la cam�ra selon le mode de vue
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        if (isFirstPerson)
        {
            // En vue premi�re personne, la cam�ra est directement � la position du joueur
            transform.position = body.position + Vector3.up * height;
            transform.rotation = rotation;
        }
        else
        {
            // En vue troisi�me personne, garde le comportement existant
            Vector3 offset = rotation * new Vector3(0, 0, -distanceFromObject);
            transform.position = body.position + Vector3.up * height + offset;
            transform.LookAt(body.position + Vector3.up * height);
        }
    }

    /// <summary>
    /// Passe de la vue de la premi�re personne � la vue � la troisi�me personne et vice versa si la camera est celle du joueur.
    /// </summary>
    private void changeView()
    {
        isFirstPerson = !isFirstPerson;
        if (isFirstPerson)
        {
            distanceFromObject = 0f;
            height = 1f;
        }
        else
        {
            distanceFromObject = 3f;
            height = 1.6f;
        }
    }
}
