using UnityEngine;

public class SFX_Button : MonoBehaviour, ButtonBase
{
    public void OnClick()
    {
        AudioManager.ins.PlaySFX(AudioManager.ins.Button_SFXClip);
    }
}
