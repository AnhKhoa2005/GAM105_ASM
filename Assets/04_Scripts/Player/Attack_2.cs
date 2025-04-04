using UnityEngine;

public class Attack_2 : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float lifeTime = 0.5f;
    [SerializeField] private GameObject thunder;
    [SerializeField] private Transform thunderPos;


    public void StartAttack()
    {
        this.gameObject.SetActive(true);
        GameObject _thunder = Instantiate(thunder, thunderPos.position, Quaternion.identity);
        _thunder.GetComponent<Thunder>().init(damage);
        Destroy(_thunder.gameObject, lifeTime);
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
            CameraShake.ins.Shake(1, 20, 0.3f);
            gameObject.GetComponent<IGethit>().Gethit(damage);
        }
    }
}
