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
            // Waves 1–2 → 5 goblins
            if (currentWave <= 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(1.5f);
                }
            }
            // Waves 3–4 → 6 goblins
            else if (currentWave <= 4)
            {
                for (int i = 0; i < 6; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(1.3f);
                }
            }
            // Waves 5–6 → 7 goblins
            else if (currentWave <= 6)
            {
                for (int i = 0; i < 7; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(1.1f);
                }
            }
            // Waves 7–8 → 8 goblins
            else if (currentWave <= 8)
            {
                for (int i = 0; i < 8; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(0.9f);
                }
            }
            // Waves 9–10 → 9 goblins
            else if (currentWave <= 10)
            {
                for (int i = 0; i < 9; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(0.8f);
                }
            }
            // Waves 11+ scale forever
            else
            {
                int goblins = 10 + (currentWave / 2);

                for (int i = 0; i < goblins; i++)
                {
                    spawner.SpawnGoblin();
                    yield return new WaitForSeconds(0.6f);
                }
            }

            // Wait until all goblins are dead
            while (GameManager.instance.goblinsAlive > 0)
            {
                yield return null;
            }

            // Next wave
            currentWave++;
            GameManager.instance.wave = currentWave;

            yield return new WaitForSeconds(2f);
        }
    }
}