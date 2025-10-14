using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform playerBody;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float distanceFromPlayer = 3f;
    [SerializeField] private float height = 1.6f;

    private CameraControls controls;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private void Awake()
    {
        controls = new CameraControls();
        controls.Camera.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Camera.Look.canceled += _ => lookInput = Vector2.zero;
    }

    private void OnEnable() => controls.Camera.Enable();
    private void OnDisable() => controls.Camera.Disable();

    private void Start()
    {
        Vector3 offset = transform.position - playerBody.position;
        yRotation = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
        xRotation = Mathf.Asin(offset.y / offset.magnitude) * Mathf.Rad2Deg;
    }

    private void LateUpdate()
    {
        // Toujours suivre le player, même sans tourner la souris
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distanceFromPlayer);
        transform.position = playerBody.position + Vector3.up * height + offset;

        transform.LookAt(playerBody.position + Vector3.up * height);

        // Tourner la caméra seulement si le bouton gauche est maintenu
        if (Mouse.current.leftButton.isPressed)
        {
            float mouseX = lookInput.x * sensitivity * Time.deltaTime;
            float mouseY = lookInput.y * sensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);
        }
    }
}
