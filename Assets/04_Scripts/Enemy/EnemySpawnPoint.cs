
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTimeFrom = 60f, spawnTimeTo = 120f;
    [SerializeField] int minCount = 1, maxCount = 5;
    private float spawnCountdown = 0, spawnCount = 0;
    private bool isInSpawnRange = false;// phải trong phạm vi spawn mới spawn đc
    void Update()
    {
        if (spawnCountdown > 0)
        {
            spawnCountdown -= Time.deltaTime;

        }
        else if (spawnCountdown <= 0 && GameManager.ins.isSpawnEnemy && isInSpawnRange) // Muốn spawn thì phải đếm ngược xong và phải dưới số lượng enemy tối đa và player phải trong vùng spawn
        {
            spawnCount = Random.Range(minCount, maxCount);
            SpawnEnemy();
            spawnCountdown = Random.Range(spawnTimeFrom, spawnTimeTo);
        }

    }

    void SpawnEnemy()
    {
        for (int i = 1; i <= spawnCount; i++)
        {
            Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
            GameManager.ins._EnemyCount++;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isInSpawnRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            isInSpawnRange = false;
        }
    }
}
