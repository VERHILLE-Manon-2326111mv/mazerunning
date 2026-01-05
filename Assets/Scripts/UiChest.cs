using UnityEngine;
using UnityEngine.UI;

public class UiChest : MonoBehaviour
{
    public static UiChest Instance { get; private set; }

    [SerializeField] private GameObject chestPanel;
    [SerializeField] private Image slotImage;
    [SerializeField] private Button slotButton;     

    private Chest currentChest;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        chestPanel.SetActive(false);
    }
    public void ToggleUi(bool show, Chest chest)
    {
        currentChest = chest;
        chestPanel.SetActive(show);
        if (show && currentChest != null)
        {
            if (currentChest.IsEmpty())
            {
                slotImage.enabled = false;
                slotButton.interactable = false;
            }
            else if(currentChest.getHasKey())
            {
                slotImage.sprite = currentChest.itemSprite;
                slotImage.enabled = true;
                slotButton.interactable = true;
            }
            else
            {
                slotImage.enabled = false;
                slotButton.interactable = false;
            }
        }
    }

    public void OnSlotButtonClick()
    {
        if (currentChest != null && !currentChest.IsEmpty())
        {
            currentChest.RetrieveItem();

            slotImage.enabled = false;
            slotButton.interactable = false;
        }
    }
}