using UnityEngine;

public class ShopToggle : MonoBehaviour
{
    public GameObject shopPanel;

    public void ToggleShop()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
    }
}