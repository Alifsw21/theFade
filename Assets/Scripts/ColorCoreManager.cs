using UnityEngine;
using TMPro;

public class ColorCoreManager : MonoBehaviour
{
    public static ColorCoreManager instance;
    private int colorCore;
    [SerializeField] private TMP_Text colorCoreDisplay;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void OnGUI()
    {
        colorCoreDisplay.text = colorCore.ToString();
    }

    public void ChangeColorCore(int amount)
    {
        colorCore += amount;
    }
}
