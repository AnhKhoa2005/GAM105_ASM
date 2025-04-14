
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI vornisText;
    [Header("-----Bar-----")]
    public Slider BGMusicBar;
    public Slider SFXBar;
    [Header("UI List")]
    public List<GameObject> UI;
    [SerializeField] Transform getUI;
    [Header("UI HUB")]
    [SerializeField] List<GameObject> _HUB;
    public Toggle MobileToggle, PCToggle;


    public GameObject minimap { get; set; }
    public bool isOnGameMode { get; set; }
    public static UIManager ins = null;

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
        ListUI();
    }

    void Update()
    {
        OpenControl();
        if (vornisText == null) return;
        ShowVornisCount();
    }

    public void UISingle(int i, int stopGame) // gọi hàm bằng script khác để mỗi khi gán scripts vào button chỉ cần tìm trong thư mục chứ không cần biết hàm ở GameObject nào trên Hierarchy
    {
        foreach (GameObject ui in UI)
        {
            ui.SetActive(false);

        }
        UI[i].SetActive(true);
        Time.timeScale = stopGame;
    }

    public void OpenControl()
    {
        if (DataManager.ins.data.MobileToggle == true)
        {
            foreach (GameObject hub in _HUB)
            {
                hub.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject hub in _HUB)
            {
                hub.SetActive(false);
            }
        }
        DataManager.ins.Save();
    }

    void ShowVornisCount()
    {
        vornisText.text = GameManager.ins.VornisCount.ToString() + "/" + GameManager.ins.vornisMax.childCount.ToString();
    }


    void ListUI() // tự gán list UI  
    {
        UI.Clear();
        for (int i = 0; i < getUI.childCount; i++)
        {
            UI.Add(getUI.GetChild(i).gameObject);
        }
    }
}