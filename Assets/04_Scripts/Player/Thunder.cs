using System;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private float damage;

    public void init(float damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Attack(col.gameObject);
    }
    void Attack(GameObject gameObject)
    {

        if (gameObject.GetComponent<IGethit>() != null)
        {
            CameraShake.ins.Shake(1, 20, 0.3f);
            gameObject.GetComponent<IGethit>().Gethit(damage);
        }
    }
}
