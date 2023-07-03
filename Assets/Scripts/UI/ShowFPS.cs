using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textFps;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        textFps.text = (1 / Time.unscaledDeltaTime).ToString("0");
    }
}