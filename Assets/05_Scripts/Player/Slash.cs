using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] private float speed;
    Rigidbody2D rb;
    float xDirCurrent;

    public void init(float xDirCurrent, float speed)
    {
        this.xDirCurrent = xDirCurrent;
        this.speed = speed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xDirCurrent > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xDirCurrent < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        rb.linearVelocity = new Vector2(speed * xDirCurrent, rb.linearVelocity.y);
    }
}
