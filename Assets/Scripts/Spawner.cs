using UnityEngine;

[System.Serializable]
public class GoblinType
{
    public GameObject prefab;
    public int weight;
}

public class Spawner : MonoBehaviour
{
    public GoblinType[] goblinTypes;

    public float[] laneY = new float[]
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

    public void SpawnGoblin()
    {
        GameObject chosenGoblin = GetRandomGoblin();

        int lane = Random.Range(0, laneY.Length);
        Vector2 spawnPos = new Vector2(10f, laneY[lane]);

        Instantiate(chosenGoblin, spawnPos, Quaternion.identity);

        GameManager.instance.GoblinSpawned();
    }

    public GameObject GetRandomGoblin()
    {
        int totalWeight = 0;

        foreach (GoblinType g in goblinTypes)
            totalWeight += g.weight;

        int random = Random.Range(0, totalWeight);

        foreach (GoblinType g in goblinTypes)
        {
            if (random < g.weight)
                return g.prefab;

            random -= g.weight;
        }

        return goblinTypes[0].prefab;
    }
}