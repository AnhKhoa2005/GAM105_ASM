using UnityEngine;

public class OpenUI_Button : MonoBehaviour, ButtonBase
{
    [SerializeField] int i, stopGame;
    public void OnClick()
    {
        UIManager.ins.UISingle(i, stopGame);
    }
}
