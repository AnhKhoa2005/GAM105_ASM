using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Data")]
public class Data : ScriptableObject
{
    public float BGMusicSource, SFXSource;
    public bool MobileToggle;
    public bool PCToggle;

}
