using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private string itemName = "Key"; 
    [SerializeField] public Sprite itemSprite;   
    
    private bool isOpen = false;
    private bool isRecupere = false;

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            UiChest.Instance.ToggleUi(true, this);
        }
        else
        {
            UiChest.Instance.ToggleUi(false, null);
        }
    }
    
    public void RetrieveItem()
    {
        isRecupere = true;
        Debug.Log($"Vous avez récupéré : {itemName} !");
    }

    public bool IsEmpty()
    {
        return isRecupere;
    }
}