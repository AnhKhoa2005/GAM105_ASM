using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header("-----Data-----")]
    public Data data;

    public static DataManager ins = null;

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
        Load();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save()
    {
        PlayerPrefs.SetFloat("BGVolume", AudioManager.ins.BGMusicSource.volume);
        PlayerPrefs.SetFloat("SFXVolume", AudioManager.ins.SFXSource.volume);
        PlayerPrefs.SetInt("MobileToggle", UIManager.ins.MobileToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("PCToggle", UIManager.ins.PCToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        //Trả về dữ liệu đã lưu ngay khi save để sử dụng ngay lập tức 
        data.BGMusicSource = PlayerPrefs.GetFloat("BGVolume");
        data.SFXSource = PlayerPrefs.GetFloat("SFXVolume");
        data.MobileToggle = PlayerPrefs.GetInt("MobileToggle") == 1 ? true : false;
        data.PCToggle = PlayerPrefs.GetInt("PCToggle") == 1 ? true : false;
    }
    public void Load()
    {
        //Trả về dữ liệu khi load scene mới
        data.BGMusicSource = PlayerPrefs.GetFloat("BGVolume");
        data.SFXSource = PlayerPrefs.GetFloat("SFXVolume");
        data.MobileToggle = PlayerPrefs.GetInt("MobileToggle") == 1 ? true : false;
        data.PCToggle = PlayerPrefs.GetInt("PCToggle") == 1 ? true : false;

        AudioManager.ins.BGMusicSource.volume = data.BGMusicSource;
        UIManager.ins.BGMusicBar.value = AudioManager.ins.BGMusicSource.volume;

        AudioManager.ins.SFXSource.volume = data.SFXSource;
        UIManager.ins.SFXBar.value = AudioManager.ins.SFXSource.volume;

        UIManager.ins.MobileToggle.isOn = data.MobileToggle;

        UIManager.ins.PCToggle.isOn = data.PCToggle;
    }

}
