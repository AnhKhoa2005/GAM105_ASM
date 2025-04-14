using System.Collections;
using UnityEngine;

public class SubmitMission_Button : MonoBehaviour, ButtonBase
{
    [SerializeField] GameObject NotificationPopUpPrefab;
    private PlayerController player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public void OnClick()
    {
        if (GameManager.ins.isMissionComplete == true)
        {

            StartCoroutine(OpenWinUI());
            NotificationPopUp("Mission complete", Color.green);
        }
        else
        {
            NotificationPopUp("Mission not complete", Color.red);
        }
    }


    void NotificationPopUp(string text, Color color)
    {
        GameObject n = Instantiate(NotificationPopUpPrefab, transform.position, Quaternion.identity);
        n.GetComponent<PopUpText>().Init(text, color);
        Destroy(n, 1f);
    }

    IEnumerator OpenWinUI()
    {
        AudioManager.ins.BGMusicSource.Stop();
        AudioManager.ins.PlaySFX(AudioManager.ins.Victory_SFXClip);
        player.gameObject.tag = "Untagged";
        player.gameObject.layer = 0;
        yield return new WaitForSeconds(2f);
        UIManager.ins.UISingle(5, 1);
    }
}
