using Edgar.Unity;
using UnityEngine;

public class FallEffect : MonoBehaviour
{
    [Header("-----Fall Effect-----")]
    [SerializeField, Tooltip("Sample: 7")] float bounceForce;
    [SerializeField, Tooltip("Sample: 0.001")] float bounceTime;
    [SerializeField, Tooltip("Sample: 1.3")] float fallTime;
    float xBounce;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        xBounce = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // Hiệu ứng nẩy lên 
        if (bounceTime > 0)
        {
            bounceTime -= Time.deltaTime;
            rb.gravityScale = 1f;
            rb.linearVelocity = new Vector2(xBounce, bounceForce);
        }
        else if (fallTime > 0)
        {
            fallTime -= Time.deltaTime;
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
