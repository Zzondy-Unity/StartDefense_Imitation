using UnityEngine;

public class Utility
{
    private static readonly string goldImage = "Images/Gold";
    private static readonly string mineralImage = "Images/Mineral";

    public static Sprite GetMoneyImage(MoneyType type)
    {
        string key = "";
        switch (type)
        {
            case MoneyType.gold:
                key = goldImage;
                break;
            case MoneyType.mineral:
                key = mineralImage;
                break;
        }

        if (key == "")
        {
            return null;
        }

        var sprite = GameManager.Resource.LoadAsset<Sprite>(key);
        if (sprite == null)
        {
            Logger.WarningLog($"{type} has no sprite");
            return null;
        }
        return sprite;
    }

    // 스프라이트가 다를경우 다른 스프라이트를 반환하는걸로 변경
    public static Color GetColorByPopupMode(UnitPopupMode mode)
    {
        Color color = Color.white;
        switch (mode)
        {
            case UnitPopupMode.Summon:
                color = Color.magenta;
                break;
            case UnitPopupMode.Exchange:
                color = Color.cyan;
                break;
            case UnitPopupMode.Transcend:
                color = Color.yellow;
                break;
        }

        if (color == Color.white)
        {
            Logger.WarningLog($"{mode} has no color");
        }

        return color;
    }
}
