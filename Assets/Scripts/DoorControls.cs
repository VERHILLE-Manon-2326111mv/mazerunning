using UnityEngine;

public class DoorControls : MonoBehaviour
{
    public void OpenDoor()
    {
        Debug.Log("La porte est ouverte !");
        gameObject.SetActive(false); 
        
    }
}