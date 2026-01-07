using UnityEngine;
using UnityEngine.InputSystem;
public class MazeSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject horizontalWall;
    [SerializeField] private GameObject verticalWall;

    void Start()
    {
        if (horizontalWall != null) horizontalWall.SetActive(true);
        if (verticalWall != null) verticalWall.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleWalls();
        }
        else if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            DesactiveWalls();
        }
    }

    void ToggleWalls()
    {
        if (horizontalWall == null || verticalWall == null) return;

        bool isHorizActive = horizontalWall.activeSelf;

        horizontalWall.SetActive(!isHorizActive);
        
        verticalWall.SetActive(isHorizActive);

        Debug.Log("SWITCH ! Labyrinthe modifié.");
    }

    void DesactiveWalls()
    {
        if (horizontalWall == null || verticalWall == null) return;
        bool isHorizActive = horizontalWall.activeSelf;
        bool isVerticalActive = verticalWall.activeSelf;

        if(isHorizActive || isVerticalActive)
        {
            horizontalWall.SetActive(false);
            verticalWall.SetActive(false);

            Debug.Log("Désactivation.");
        }
        else
        {
            verticalWall.SetActive(true);
            Debug.Log("Réactivation.");
        }
        

    }
}