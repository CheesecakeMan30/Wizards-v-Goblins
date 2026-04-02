using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int goblinsKilled = 0;
    public int goblinsAlive = 0;
    public int wave = 1;
    public bool gameOver = false;
    
    void Awake()
    {
        instance = this;
    }

    public void GoblinSpawned()
    {
        goblinsAlive++;
    }

    public void GoblinKilled()
    {
        goblinsKilled++;
        goblinsAlive--;
    }
}