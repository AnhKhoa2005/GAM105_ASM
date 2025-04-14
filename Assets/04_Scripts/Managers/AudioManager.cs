using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{


    [Header("-----Audio Sources-----")]
    public AudioSource BGMusicSource;
    public AudioSource SFXSource;
    [Header("-----Audio Clips-----")]
    public AudioClip BGMusicClip;
    public AudioClip Run_SFXClip;
    public AudioClip Skill1_SFXClip;
    public AudioClip Skill2_SFXClip;
    public AudioClip Dash_SFXClip;
    public AudioClip Gethit_SFXClip;
    public AudioClip Death_SFXClip;
    public AudioClip EnemySkill_SFXClip;
    public AudioClip HP_SFXClip;
    public AudioClip Vornis_SFXClip;
    public AudioClip Chest_SFXClip;
    public AudioClip Button_SFXClip;
    public AudioClip Victory_SFXClip;

    public static AudioManager ins = null;

    void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BGMusicSource.clip = BGMusicClip;
        BGMusicSource.Play();
    }

    void Update()
    {
        ChangeBGMusicVolume();
    }
    void ChangeBGMusicVolume()
    {
        BGMusicSource.volume = UIManager.ins.BGMusicBar.value;
        SFXSource.volume = UIManager.ins.SFXBar.value;
        DataManager.ins.Save();
    }

    public void PlaySFX(AudioClip sfx)
    {
        SFXSource.PlayOneShot(sfx);
    }
}
