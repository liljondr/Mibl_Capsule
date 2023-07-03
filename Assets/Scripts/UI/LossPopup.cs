using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LossPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    public void SetGameTime(TimeSpan gameTime)
    {
        string time = new DateTime(gameTime.Ticks).ToString("mm:ss");
        timeText.text = $"<size=70%>The game lasted</size> \n{time}";
    }
}