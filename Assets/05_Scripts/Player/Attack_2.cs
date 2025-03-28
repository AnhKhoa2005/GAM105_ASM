using UnityEngine;

public class Attack_2 : MonoBehaviour
{
    [SerializeField] private GameObject slashPrefab;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float speed = 15;

    public float xDirCurrent { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAttack()
    {
        GameObject _slash = Instantiate(slashPrefab, attackPoint.position, Quaternion.identity);
        _slash.GetComponent<Slash>().init(xDirCurrent, speed);
        Destroy(_slash, 1f);
    }
}
