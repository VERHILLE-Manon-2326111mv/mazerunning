using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RandomKeyManager : MonoBehaviour
{
    public static RandomKeyManager Instance;

    [Header("Configuration")]
    [SerializeField] private DoorControls door;
    [SerializeField] private int keysRequired = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI keyNumberText;

    private int keysCollected = 0;
    private List<Chest> allChests = new List<Chest>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Chest[] foundChests = FindObjectsOfType<Chest>();
        allChests.AddRange(foundChests);
        DistributeKeys();
    }

    void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            Debug.Log("CHEAT ACTIVÉ");
            keysCollected = keysRequired;
            keyNumberText.text = keysCollected + " / 3";
            CheckWin();
        }
    }

    void DistributeKeys()
    {
        if (allChests.Count < keysRequired) return;

        List<Chest> chestPool = new List<Chest>(allChests);

        for (int i = 0; i < keysRequired; i++)
        {
            int randomIndex = Random.Range(0, chestPool.Count);
            chestPool[randomIndex].SetHasKey(true);
            chestPool.RemoveAt(randomIndex);
        }
    }

    public void AddKey()
    {
        keysCollected++;
        Debug.Log($" Clé trouvée ! ({keysCollected}/{keysRequired})");
        CheckWin();
    }

    /// <summary>
    // Met à jour le texte à l'écran
    /// </summary>
    private void UpdateKeyUI()
    {
        if (keyNumberText != null)
        {
            keyNumberText.text = keysCollected + " / 3";
        }
    }

    private void CheckWin()
    {
        if (keysCollected >= keysRequired)
        {
            if (door != null) door.OpenDoor();
        }
    }
}