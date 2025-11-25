using System;
using UnityEngine;
using UnityEngine.UI;

public class cell_bounty : MonoBehaviour
{
   [SerializeField] private atom_ImgTmp IT;
   [SerializeField] private Button btn;
   private int curBounty;
   public atom_ImgTmp It { get; private set; }

   public void Init(Sprite sprite, string text, int curBounty, Action<int> onClick)
   {
      if (IT == null)
      {
         IT = GetComponent<atom_ImgTmp>();
      }

      if (btn == null)
      {
         btn = GetComponent<Button>();
      }
      
      IT.Set(sprite, text);
      btn.onClick.RemoveAllListeners();
      btn.onClick.AddListener(() => onClick?.Invoke(curBounty));
      this.curBounty = curBounty;
   }
}
