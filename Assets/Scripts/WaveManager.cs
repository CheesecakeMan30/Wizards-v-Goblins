using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Spawner spawner;

    int currentWave = 1;
    bool waitingForNextWave = false;

    // 🔥 Lane system
    public float[] laneY = new float[]
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

    public float minLaneSpacing = 1.2f;   // distance between goblins in same lane
    public float spawnX = 10f;            // spawn position X

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
            float spawnDelay = Mathf.Max(0.4f, 1.5f - currentWave * 0.08f);

            AdjustGoblinWeights();

            for (int i = 0; i < goblinsToSpawn; i++)
            {
                yield return StartCoroutine(SpawnInLane(spawnDelay));
            }

            // Wait until all goblins are dead
            while (GameObject.FindGameObjectsWithTag("Goblin").Length > 0)
            {
                yield return null;
            }

            currentWave++;
            GameManager.instance.wave = currentWave;

            GameManager.instance.ClearProjectiles();

            GamePause.instance.PauseGame();
            GameManager.instance.waveCompleteUI.SetActive(true);

            waitingForNextWave = true;
            yield return new WaitUntil(() => waitingForNextWave == false);
        }
    }

    // 🔥 NEW: lane-based spawn system
    IEnumerator SpawnInLane(float delay)
    {
        bool spawned = false;

        while (!spawned)
        {
            int laneIndex = Random.Range(0, laneY.Length);
            float lane = laneY[laneIndex];

            if (IsLaneClear(lane))
            {
                SpawnGoblinInLane(lane);
                spawned = true;
            }

            if (!spawned)
                yield return null;
        }

        yield return new WaitForSeconds(delay);
    }

    // 🔥 Check if lane has space near spawn
    bool IsLaneClear(float laneYPos)
    {
        GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");

        foreach (GameObject g in goblins)
        {
            if (g == null) continue;

            float dy = Mathf.Abs(g.transform.position.y - laneYPos);

            // same lane
            if (dy < 0.3f)
            {
                // too close to spawn area
                if (g.transform.position.x > spawnX - minLaneSpacing)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // 🔥 Force spawn in specific lane
    void SpawnGoblinInLane(float laneYPos)
    {
        GameObject goblinPrefab = spawner.GetRandomGoblin(); // make this public

        Vector2 spawnPos = new Vector2(spawnX, laneYPos);

        Instantiate(goblinPrefab, spawnPos, Quaternion.identity);

        GameManager.instance.GoblinSpawned();
    }

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