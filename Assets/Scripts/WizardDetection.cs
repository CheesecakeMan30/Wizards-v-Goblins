using UnityEngine;

public class WizardDetection : MonoBehaviour
{
    private Wizard wizard;

    void Awake()
    {
        // 🔥 More reliable than Start
        wizard = GetComponentInParent<Wizard>();

        if (wizard == null)
        {
            Debug.LogError("WizardDetection could not find Wizard!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (wizard == null) return;

        if (other.CompareTag("Goblin"))
        {
            wizard.EnemyEntered(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (wizard == null) return;

        if (other.CompareTag("Goblin"))
        {
            wizard.EnemyExited(other.gameObject);
        }
    }
}