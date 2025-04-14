using UnityEngine;

public class Minimap_Button : MonoBehaviour, ButtonBase
{
    bool isZoomIn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.ins.minimap = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        OpenMinimapByKeyboard();
    }

    public void OpenMinimapByKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isZoomIn)
        {
            ZoomInMap();
            isZoomIn = true;
        }
        else if (Input.GetKeyDown(KeyCode.M) && isZoomIn)
        {
            ZoomOutMap();
            isZoomIn = false;
        }
    }

    public void OnClick()
    {
        if (!isZoomIn)
        {
            ZoomInMap();
            isZoomIn = true;
        }
        else if (isZoomIn)
        {
            ZoomOutMap();
            isZoomIn = false;
        }
    }
    public void ZoomInMap()
    {
        UIManager.ins.minimap.GetComponent<RectTransform>().localScale = new Vector3(3f, 3f, 1);
    }

    public void ZoomOutMap()
    {
        UIManager.ins.minimap.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}
