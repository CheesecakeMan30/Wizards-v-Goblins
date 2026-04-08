using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public Spawner spawner;

    int currentWave = 1;

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

        for (int i = 0; i < goblinsToSpawn; i++)
        {
            spawner.SpawnGoblin();
            yield return new WaitForSeconds(spawnDelay);
        }

        while (GameManager.instance.goblinsAlive > 0)
        {
            yield return null;
        }

        currentWave++;
        GameManager.instance.wave = currentWave;

        yield return new WaitForSeconds(2f);
    }
}

  void AdjustGoblinWeights()
{

    if (currentWave < 5)
    {
        spawner.goblinTypes[0].weight = 90; // weak
        spawner.goblinTypes[1].weight = 10; // mid
        spawner.goblinTypes[2].weight = 0;  // strong locked
    }
    else if (currentWave < 10)
    {
        spawner.goblinTypes[0].weight = 70;
        spawner.goblinTypes[1].weight = 30;
        spawner.goblinTypes[2].weight = 0;
    }
    else if(currentWave < 15)
    {
        spawner.goblinTypes[0].weight = 40;
        spawner.goblinTypes[1].weight = 40;
        spawner.goblinTypes[2].weight = 20;
    }
    else if(currentWave < 20)
    {
        spawner.goblinTypes[0].weight = 15;
        spawner.goblinTypes[1].weight = 40;
        spawner.goblinTypes[2].weight = 55;
    }
    else if(currentWave < 25)
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