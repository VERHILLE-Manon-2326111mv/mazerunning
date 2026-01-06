using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalControl : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject portal;
    [SerializeField] private float teleportDelay = 0f;
    [SerializeField] private ParticleSystem teleportEffect;

    private GameObject currentPortal;
    private bool isTeleporting = false;
    private Rigidbody rb;
    private PlayerScript playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame && !isTeleporting)
        {
            HandlePortalInput();
        }
    }

    void HandlePortalInput()
    {
        if (currentPortal == null)
        {
            SpawnPortal();
        }
        else
        {
            StartCoroutine(TeleportSequence());
        }
    }

    void SpawnPortal()
    {
        RaycastHit hit;
        Vector3 spawnPos = transform.position;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f))
        {
            spawnPos = hit.point + new Vector3(0, 0.02f, 0);
        }
        else
        {
            spawnPos = new Vector3(transform.position.x, 0.02f, transform.position.z);
        }

        currentPortal = Instantiate(portal, spawnPos, Quaternion.identity);
        Debug.Log("Portail posé !");
    }

    IEnumerator TeleportSequence()
    {
        isTeleporting = true;
        Debug.Log("⏳ Téléportation imminente...");

        if (playerMovement != null) playerMovement.SetCanMove(false);

        if (teleportEffect != null) teleportEffect.Play();

        yield return new WaitForSeconds(teleportDelay);

        rb.velocity = Vector3.zero; 
        Vector3 targetPos = new Vector3(currentPortal.transform.position.x, transform.position.y, currentPortal.transform.position.z);
        transform.position = targetPos;

        Debug.Log("✨ Téléportation réussie !");

        if (playerMovement != null) playerMovement.SetCanMove(true);
        isTeleporting = false;
    }


}