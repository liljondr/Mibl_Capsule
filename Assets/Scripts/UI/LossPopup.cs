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
    [SerializeField] private Image background;

    private float animationTime = 1.5f;
    private float startBGposition;
    private float endBGposition ;
    private Color startColorBG;
    private Color endColorBG;
    private Color endColorText;
    private float endalpha = 0.3f;
    
    public void SetGameTime(TimeSpan gameTime)
    {
        
            string time= new DateTime(gameTime.Ticks).ToString("mm:ss");
            timeText.text = $"<size=70%>The game lasted</size> \n{time}";

    }
}
