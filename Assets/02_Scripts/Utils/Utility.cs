using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utility
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
            case UnitPopupMode.FixTile:
                color = Color.green;
                break;
        }

        if (color == Color.white)
        {
            Logger.WarningLog($"{mode} has no color");
        }

        return color;
    }

    public static Color GetColorByGrade(Grade grade)
    {
        Color color = Color.white;

        switch (grade)
        {
            case Grade.common:
                color = Color.white;
                break;
            case Grade.normal:
                color = Color.green;
                break;
            case Grade.rare:
                color = Color.blue;
                break;
            case Grade.epic:
                color = Color.magenta;
                break;
            case Grade.legendary:
                color = Color.yellow;
                break;
        }

        return color;
    }

    public static bool Contains(this LayerMask layerMask, int layer)
    {
        return (layerMask.value & (1 << layer)) != 0;
    }

    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            return false;
        }
        
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}