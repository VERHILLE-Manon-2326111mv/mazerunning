using UnityEngine;
using UnityEngine.InputSystem;

public class ChestInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRange = 2f;
    private Chest chestInRange;

    void Update()
    {
        DetectChest();

        if (Keyboard.current.eKey.wasPressedThisFrame && chestInRange != null){
            chestInRange.Interact();
        }
    }

    private void DetectChest()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        chestInRange = null;
        foreach (var hit in hits)
        {
            Chest chest = hit.GetComponent<Chest>();
            if (chest != null)
            {
                chestInRange = chest;
                break;
            }
        }
    }
}
