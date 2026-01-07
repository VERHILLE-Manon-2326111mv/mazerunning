using UnityEngine;

public class AutoTiling : MonoBehaviour
{
    [Header("Réglages")]
    [Tooltip("Ajuste ce chiffre pour grossir/réduire les briques")]
    public float textureDensity = 0.2f; 

    void Start()
    {
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in allRenderers)
        {
            Vector3 scale = rend.transform.lossyScale;
            float length = Mathf.Max(scale.x, scale.z);
            float height = scale.y;

            float tilingX = length * textureDensity;
            float tilingY = height * textureDensity;

            if (rend.material != null)
            {
                rend.material.mainTextureScale = new Vector2(tilingX, tilingY);
            }
        }
    }
}