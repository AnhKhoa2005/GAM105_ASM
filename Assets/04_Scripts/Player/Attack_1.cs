using UnityEngine;

public class Attack_1 : MonoBehaviour
{
    [SerializeField] private float damage = 10;

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
        this.gameObject.SetActive(true);
    }

    public void EndAttack()
    {
        this.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Attack(col.gameObject);
    }
    void Attack(GameObject gameObject)
    {

        if (gameObject.GetComponent<IGethit>() != null)
        {
            CameraShake.ins.Shake(1f, 20, 0.3f);
            gameObject.GetComponent<IGethit>().Gethit(damage);
        }

        if (gameObject.GetComponent<Entity>() != null)
        {
            gameObject.GetComponent<Entity>()._PushBack(this.transform.position);
        }
    }
}
