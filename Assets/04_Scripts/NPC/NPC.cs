using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject chatBox;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            chatBox.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            chatBox.SetActive(false);
        }
    }
}
