using System.Collections;
using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField] float _HP = 3f;

    void OnTriggerEnter2D(Collider2D col)
    {
        GetHP(col.gameObject);
    }
    void GetHP(GameObject gameObject)
    {

        if (gameObject.GetComponent<IGetHP>() != null)
        {
            AudioManager.ins.PlaySFX(AudioManager.ins.HP_SFXClip);
            gameObject.GetComponent<IGetHP>().GetHP(_HP);
            Destroy(this.gameObject);
        }
    }
}

