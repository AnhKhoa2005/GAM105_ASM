using UnityEngine;

public class KeepFlip : MonoBehaviour
{
    [SerializeField] private Entity entity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (entity.movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
