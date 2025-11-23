using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileNode
{
    public TileType tileType;
    public Vector3Int cellPos;
    public Vector3 worldPos;

    public TileNode parent;

    public Hero curHero;

    public void Interact(Vector2 screenPos)
    {
        UIBase ui;
        if (curHero == null)
        {
            // 영웅소환 ui 생성
            ui = GameManager.UI.Show<UISummon>(this);
            ui.transform.position = screenPos;
            return;
        }
        else
        {
            if (GameManager.Scene.curSceneManager is GameSceneManager gameScene)
            {
                if (curHero.heroGrade == Grade.epic)
                {
                    // 그.. 뮤리? 그런 빨간색 바닥 나오는 최종형태로 업그레이드 가능하게 하는 ui 생성
                    return;
                }
                if( gameScene.Stage.CanUpgrade(curHero.data))
                {
                    // 영웅 승급 ui 생성
                    return;
                }
            }
                
        }
    }
}
