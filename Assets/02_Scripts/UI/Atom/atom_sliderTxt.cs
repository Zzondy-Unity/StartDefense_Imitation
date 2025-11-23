using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_sliderTxt : MonoBehaviour
{
   [SerializeField] private Slider slider;
   [SerializeField] private TextMeshProUGUI text;

   public float maxValue {get => slider.maxValue; set => slider.maxValue = value;}
   public float value
   {
      get => slider.value;
      set
      {
         slider.value = value;
         Refresh();
      }
   }

   private void Refresh()
   {
      text.text = $"( {slider.value} / {slider.maxValue} )";
   }
}
