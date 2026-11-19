
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private GameObject inventoryMenu;
    [SerializeField] private Key inventoryKey = Key.Tab;

    private bool menuActivated;

    private void Start()
    {
        if (inventoryMenu != null)
        {
            inventoryMenu.SetActive(false);
            menuActivated = false;
        }
    }

    private void Update()
    {
        if (Keyboard.current == null || inventoryMenu == null)
            return;

        if (Keyboard.current[inventoryKey].wasPressedThisFrame)
        {
            menuActivated = !menuActivated;
            inventoryMenu.SetActive(menuActivated);
        }
    }
}
