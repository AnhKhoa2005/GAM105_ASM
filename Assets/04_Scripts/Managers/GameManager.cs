using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float EnemyCountMax = 50;
    public Transform vornisMax;
    public bool isSpawnEnemy { get; set; } = true;
    public int _EnemyCount { get; set; } = 0;
    public int VornisCount { get; set; } = 0;
    public bool isMissionComplete { get; set; } = false;

    public static GameManager ins = null;

    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyCount();
        if (vornisMax == null) return;
        MissionComplete();
    }

    void EnemyCount()
    {
        if (_EnemyCount > EnemyCountMax)
        {
            isSpawnEnemy = false;
        }
        else
        {
            isSpawnEnemy = true;
        }
    }

    void MissionComplete()
    {
        if (VornisCount >= vornisMax.childCount)
        {
            isMissionComplete = true;
        }
    }
}
