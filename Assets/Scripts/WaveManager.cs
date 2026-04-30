using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Spawner spawner;

    int currentWave = 1;
    bool waitingForNextWave = false;

    public int winWave = 26;

    public float[] laneY = new float[]
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

    public float minLaneSpacing = 1.2f;
    public float spawnX = 10f;

    public GameObject startWaveText;
    public float introDelay = 3f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(StartSequence());
    }


    IEnumerator StartSequence()
    {
        if (startWaveText != null)
            startWaveText.SetActive(true);

        yield return new WaitForSeconds(introDelay);

        if (startWaveText != null)
            startWaveText.SetActive(false);

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

            while (GameObject.FindGameObjectsWithTag("Goblin").Length > 0)
            {
                yield return null;
            }

            currentWave++;
            GameManager.instance.wave = currentWave;

            GameManager.instance.ClearProjectiles();

            if (currentWave == winWave)
            {
                GameManager.instance.ShowWinScreen();

                waitingForNextWave = true;
                yield return new WaitUntil(() => waitingForNextWave == false);

                continue;
            }

            GamePause.instance.PauseGame(false);
            GameManager.instance.waveCompleteUI.SetActive(true);

            waitingForNextWave = true;
            yield return new WaitUntil(() => waitingForNextWave == false);
        }
    }

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

    bool IsLaneClear(float laneYPos)
    {
        GameObject[] goblins = GameObject.FindGameObjectsWithTag("Goblin");

        foreach (GameObject g in goblins)
        {
            if (g == null) continue;

            float dy = Mathf.Abs(g.transform.position.y - laneYPos);

            if (dy < 0.3f)
            {
                if (g.transform.position.x > spawnX - minLaneSpacing)
                {
                    return false;
                }
            }
        }

        return true;
    }

    void SpawnGoblinInLane(float laneYPos)
    {
        GameObject goblinPrefab = spawner.GetRandomGoblin();

        Vector2 spawnPos = new Vector2(spawnX, laneYPos);

        GameObject goblin = Instantiate(goblinPrefab, spawnPos, Quaternion.identity);

        goblin.GetComponent<GoblinMovement>().laneY = laneYPos;

        GameManager.instance.GoblinSpawned();
    }

    public void ContinueNextWave()
    {
        GameManager.instance.waveCompleteUI.SetActive(false);
        GamePause.instance.ResumeGame();

        waitingForNextWave = false;
    }

    public void ContinueAfterWin()
    {
        waitingForNextWave = false;
    }

    void AdjustGoblinWeights()
    {
        if (currentWave < 5)
        {
            spawner.goblinTypes[0].weight = 90;
            spawner.goblinTypes[1].weight = 10;
            spawner.goblinTypes[2].weight = 0;
            spawner.goblinTypes[3].weight = 0;
            spawner.goblinTypes[4].weight = 0;
        }
        else if (currentWave < 10)
        {
            spawner.goblinTypes[0].weight = 70;
            spawner.goblinTypes[1].weight = 30;
            spawner.goblinTypes[2].weight = 0;
            spawner.goblinTypes[3].weight = 0;
            spawner.goblinTypes[4].weight = 0;
        }
        else if (currentWave < 15)
        {
            spawner.goblinTypes[0].weight = 40;
            spawner.goblinTypes[1].weight = 40;
            spawner.goblinTypes[2].weight = 20;
            spawner.goblinTypes[3].weight = 0;
            spawner.goblinTypes[4].weight = 0;
        }
        else if (currentWave < 20)
        {
            spawner.goblinTypes[0].weight = 15;
            spawner.goblinTypes[1].weight = 40;
            spawner.goblinTypes[2].weight = 35;
            spawner.goblinTypes[3].weight = 10;
            spawner.goblinTypes[4].weight = 0;
        }
        else if (currentWave < 25)
        {
            spawner.goblinTypes[0].weight = 0;
            spawner.goblinTypes[1].weight = 25;
            spawner.goblinTypes[2].weight = 35;
            spawner.goblinTypes[3].weight = 25;
            spawner.goblinTypes[4].weight = 15;
        }
        else
        {
            spawner.goblinTypes[0].weight = 0;
            spawner.goblinTypes[1].weight = 10;
            spawner.goblinTypes[2].weight = 30;
            spawner.goblinTypes[3].weight = 30;
            spawner.goblinTypes[4].weight = 30;
        }
    }
}