using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;

    bool isZoomIn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Minimap();
    }

    public void Minimap()
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
    public void ZoomInMap()
    {
        miniMap.GetComponent<RectTransform>().localScale = new Vector3(3.3f, 3.3f, 1);
    }

    public void ZoomOutMap()
    {
        miniMap.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}
