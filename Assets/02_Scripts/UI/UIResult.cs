using TMPro;
using UnityEngine;

public class UIResult : UIPopUp
{
   [SerializeField] private TextMeshProUGUI resultText;
   [SerializeField] private atom_btn toNextScene;

   public override void Opened(object[] param)
   {
      base.Opened(param);

      if (param[0] is bool isWin)
      {
         resultText.text = isWin ? "VICTORY!" : "FAIL";
         toNextScene.Set("ToNextScene", OnClick);
      }
   }

   private void OnClick()
   {
      Hide<UIResult>();
      GameManager.Scene.LoadScene(SceneName.StartScene);
   }
}
