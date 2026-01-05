using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] public Sprite itemSprite;   

    private bool hasKey = false;    
    private bool isOpen = false;
    private bool isRecupere = false;

    public void SetHasKey(bool status)
    {
            hasKey = status;
    }

    public bool getHasKey()
    {
        return hasKey;
    }

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
        if (isRecupere) return;
        isRecupere = true;
        if (hasKey)
        {
            Debug.Log("BRAVO : Vous avez trouvé une clé !");
            RandomKeyManager.Instance.AddKey();        
        }
        else
        {
            Debug.Log("Ce coffre est malheuresement vide...");   
        }
    }

    public bool IsEmpty()
    {
        return isRecupere;
    }
}