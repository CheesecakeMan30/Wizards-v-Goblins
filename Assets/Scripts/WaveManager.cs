using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Spawner spawner;

    int currentWave = 1;

    bool waitingForNextWave = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (!GameManager.instance.gameOver)
        {
            int goblinsToSpawn = 3 + currentWave * 2;
            float spawnDelay = Mathf.Max(0.3f, 1.5f - currentWave * 0.1f);

            AdjustGoblinWeights();

            //  Spawn wave
            for (int i = 0; i < goblinsToSpawn; i++)
            {
                spawner.SpawnGoblin();
                yield return new WaitForSeconds(spawnDelay);
            }

            //  Wait until all goblins are dead
         while (GameObject.FindGameObjectsWithTag("Goblin").Length > 0)
            {
                 yield return null;
            }

            //  Wave complete
            currentWave++;
            GameManager.instance.wave = currentWave;

            // Clear all projectiles FIRST
            GameManager.instance.ClearProjectiles();

            // Then pause + show UI
            GamePause.instance.PauseGame();
            GameManager.instance.waveCompleteUI.SetActive(true);

            //  WAIT for player input
            waitingForNextWave = true;
            yield return new WaitUntil(() => waitingForNextWave == false);
        }
    }

    //  Called by button
    public void ContinueNextWave()
    {
        GameManager.instance.waveCompleteUI.SetActive(false);
        GamePause.instance.ResumeGame();

        waitingForNextWave = false;
    }

    void AdjustGoblinWeights()
    {
        if (currentWave < 5)
        {
            spawner.goblinTypes[0].weight = 90;
            spawner.goblinTypes[1].weight = 10;
            spawner.goblinTypes[2].weight = 0;
        }
        else if (currentWave < 10)
        {
            spawner.goblinTypes[0].weight = 70;
            spawner.goblinTypes[1].weight = 30;
            spawner.goblinTypes[2].weight = 0;
        }
        else if (currentWave < 15)
        {
            spawner.goblinTypes[0].weight = 40;
            spawner.goblinTypes[1].weight = 40;
            spawner.goblinTypes[2].weight = 20;
        }
        else if (currentWave < 20)
        {
            spawner.goblinTypes[0].weight = 15;
            spawner.goblinTypes[1].weight = 40;
            spawner.goblinTypes[2].weight = 55;
        }
        else if (currentWave < 25)
        {
            spawner.goblinTypes[0].weight = 0;
            spawner.goblinTypes[1].weight = 30;
            spawner.goblinTypes[2].weight = 70;
        }
        else
        {
            spawner.goblinTypes[0].weight = 0;
            spawner.goblinTypes[1].weight = 15;
            spawner.goblinTypes[2].weight = 85;
        }
    }
}