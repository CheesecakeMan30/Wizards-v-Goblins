using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject goblinPrefab;

    public float[] laneY = new float[]
    {
        3f, 1.75f, 0.5f, -1f, -2.25f, -3.5f
    };

   public void SpawnGoblin()
{
    int lane = Random.Range(0, laneY.Length);
    Vector2 spawnPos = new Vector2(10f, laneY[lane]);

    Instantiate(goblinPrefab, spawnPos, Quaternion.identity);

    GameManager.instance.GoblinSpawned();
}
}