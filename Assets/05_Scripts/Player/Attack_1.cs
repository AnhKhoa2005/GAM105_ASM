using UnityEngine;

public class Attack_1 : MonoBehaviour
{
    Collider2D attackCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackCollider = GetComponent<Collider2D>();
        attackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAttack()
    {
        attackCollider.enabled = true;
    }

    public void EndAttack()
    {
        attackCollider.enabled = false;
    }
}
