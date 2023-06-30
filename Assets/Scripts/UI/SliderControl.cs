using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour
{
   [SerializeField] private Slider mySlider;
   [SerializeField] private TextMeshProUGUI valueText;

  
   public void SetValue(float value)
   {
      mySlider.value = value;
   }

   public void ChangeTextSpawnDelay(float spawnDelay)
   {
      valueText.text = spawnDelay.ToString("0.0");
   }
}
