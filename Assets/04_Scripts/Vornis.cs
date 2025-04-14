using UnityEngine;

public class Vornis : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            AudioManager.ins.PlaySFX(AudioManager.ins.Vornis_SFXClip);
            GameManager.ins.VornisCount++;
            Destroy(gameObject);
        }
    }
}
