using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LossPopup lossPopup;
    [SerializeField] private SliderControl speedSlider;
    [SerializeField] private SliderControl spawnDelaySlider;
    public event Action<float> OnChangeSpeed;
    public event Action<float> OnChangeSpawnDelay;
    public event Action OnPressReplayButton;

    /// <summary>
    ///Index  corrects the speed value. So that it does not acquire values less than 0.1
    /// </summary>
    private float correctMultiplay = 100;

    private void Start()
    {
        lossPopup.gameObject.SetActive(false);
    }

    #region button listening
    public void OpenLossPopup(TimeSpan gameTime)
    {
        lossPopup.gameObject.SetActive(true);
        lossPopup.SetGameTime(gameTime);
    }

    public void OnChangeSpeedSlider(float value)
    {
        OnChangeSpeed?.Invoke(value);
    }

    public void OnChangeSpawnDelaySlider(float value)
    {
        OnChangeSpawnDelay?.Invoke(value);
    }
    #endregion

    public void SetSpawnDelayValue(float lerpSpawnDelay)
    {
        spawnDelaySlider.SetValue(lerpSpawnDelay);
    }

    public void SetSpawnDelayText(float spawnDelay)
    {
        spawnDelaySlider.ChangeTextSpawnDelay(spawnDelay);
    }

    public void SetSpeedText(float speed)
    {
        speedSlider.ChangeTextSpawnDelay(speed * correctMultiplay);
    }

    public void SetSpeedValue(float lerpSeed)
    {
        speedSlider.SetValue(lerpSeed);
    }

    public void OnPressReplay()
    {
        lossPopup.gameObject.SetActive(false);
        OnPressReplayButton?.Invoke();
    }
}