using UnityEngine;
using UnityEngine.UI; // Ne pas oublier ce using !

public class UiChest : MonoBehaviour
{
    public static UiChest Instance { get; private set; }

    [SerializeField] private GameObject chestPanel;
    [SerializeField] private Image slotImage;
    [SerializeField] private Button slotButton; // On garde la référence pour le désactiver

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
        if (show)
        {
            if (currentChest.IsEmpty())
            {
                slotImage.enabled = false;
                slotButton.interactable = false;
            }
            else
            {
                slotImage.sprite = currentChest.itemSprite;
                slotImage.enabled = true;
                slotButton.interactable = true;
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