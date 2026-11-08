using UnityEngine;

public class ThymeSpawner : MonoBehaviour
{
    public bool cats;
    [SerializeField] int baseSpawnCount = 40;
    [SerializeField] Vector2 spawnAreaMin;
    [SerializeField] Vector2 spawnAreaMax;
    [SerializeField] private GameObject thymePrefab;

    private void Start()
    {
        if (cats) baseSpawnCount += SaveSystem.Instance.Data.CatAdditive;
        else baseSpawnCount += SaveSystem.Instance.Data.ThymeAdditive;

        for (int i = 0; i < baseSpawnCount; i++)
        {
            var spawnPosition = new Vector2(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            if (Random.Range(0f, 1f) < 0.5f) spawnPosition.x *= -1;
            if (Random.Range(0f, 1f) < 0.5f) spawnPosition.y *= -1;

            Instantiate(thymePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
