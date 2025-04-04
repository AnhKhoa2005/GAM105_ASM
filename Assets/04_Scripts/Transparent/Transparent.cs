using UnityEngine;
using UnityEngine.Tilemaps;

public class Transparent : MonoBehaviour
{
    SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Enemy"))
        {
            if (col.transform.position.y > this.transform.position.y - 1)
            {
                Color color = sr.color;
                color.a = 0.4f;
                sr.color = color;
                sr.sortingOrder = 10;
            }
            else
            {
                Color color = sr.color;
                color.a = 1;
                sr.color = color;
                sr.sortingOrder = 4;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Enemy"))
        {
            Color color = sr.color;
            color.a = 1;
            sr.color = color;
            sr.sortingOrder = 4;
        }
    }
}
