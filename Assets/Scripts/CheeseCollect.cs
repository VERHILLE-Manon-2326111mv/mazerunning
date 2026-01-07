using UnityEngine;

public class CheeseCollect : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private AudioClip crunchSound;
    [SerializeField] [Range(0f, 1f)] private float soundVolume = 0.8f;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (crunchSound != null)
        {
            AudioSource.PlayClipAtPoint(crunchSound, transform.position, soundVolume);
        }

        Debug.Log("Miam ! Fromage récupéré.");

        Destroy(gameObject);
    }
}