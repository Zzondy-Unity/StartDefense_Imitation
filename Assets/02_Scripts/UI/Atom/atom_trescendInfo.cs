using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_trescendInfo : MonoBehaviour
{
   [SerializeField] private Image unit;
   [SerializeField] private Image block;
   [SerializeField] private TextMeshProUGUI desc;
   [SerializeField] private Button thisBtn;

   public void Set(HeroData data, bool can, Action<int> onClick)
   {
      unit.sprite = GameManager.Resource.LoadAsset<Sprite>(data.spriteKey);
      block.gameObject.SetActive(!can);
      desc.text = data.heroName;
      thisBtn.interactable = can;
      thisBtn.onClick.RemoveAllListeners();
      thisBtn.onClick.AddListener(() => onClick?.Invoke(data.heroId));
      
   }
}
