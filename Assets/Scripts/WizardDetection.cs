using UnityEngine;

public class WizardDetection : MonoBehaviour
{
    Wizard wizard;

    void Start()
    {
        wizard = GetComponentInParent<Wizard>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goblin"))
        {
            wizard.EnemyEntered();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Goblin"))
        {
            wizard.EnemyExited();
        }
    }
}