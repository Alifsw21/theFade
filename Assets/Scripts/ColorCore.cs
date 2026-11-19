using UnityEngine;

public class ColorCore : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private ColorCoreManager colorCoreManager;

    private void Start()
    {
        colorCoreManager = ColorCoreManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            colorCoreManager.ChangeColorCore(value);
            Destroy(gameObject);
        }
    }
}
