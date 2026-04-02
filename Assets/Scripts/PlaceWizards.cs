using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject wizardPrefab;
    private bool occupied = false;

    void OnMouseDown()
    {
        if (!occupied)
        {
            Instantiate(wizardPrefab, transform.position, Quaternion.identity);
            occupied = true;
        }
    }
}