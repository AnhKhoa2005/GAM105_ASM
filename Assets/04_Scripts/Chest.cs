using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    bool isOpen = false;
    Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ani.CrossFade("Opened", 0f);
            AudioManager.ins.PlaySFX(AudioManager.ins.Chest_SFXClip);
            if (isOpen) return;
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
            isOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ani.CrossFade("Closed", 0f);
        }
    }
}
