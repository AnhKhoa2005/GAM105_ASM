using UnityEngine;

public class Quit_Button : MonoBehaviour, ButtonBase
{
    public void OnClick()
    {
        Application.Quit();
    }
}
