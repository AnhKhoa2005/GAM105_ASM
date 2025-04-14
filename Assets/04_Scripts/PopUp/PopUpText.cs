using System;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI popupText;

    public void Init(string text, Color color)
    {
        popupText.text = text;
        popupText.color = color;
    }
}
