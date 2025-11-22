public enum TileType
{
    Block,      // 고칠 수 없는 타일
    Wall,       // 벽
    Spawn,      // 몬스터 스폰 타일
    Goal,       // 몬스터 목표 타일
    Normal,     // 영웅 스폰 가능 타일
    FixTile,    // 고칠 수 있는 
    Road,       // 지나갈 수 있는 빈 공간
}

public enum BGM
{
    
}

public enum SFX
{
    
}

public enum Grade
{
    common = 1, //흰색
    normal,     //초록
    rare,       //파랑
    epic,       //보라
    legendary,  //노랑
}

public enum SceneName
{
    StartScene,
    LobbyScene,
    GameScene,
}

public enum MoneyType
{
    gold,
    mineral,
}

public enum UnitPopupMode
{
    Summon,
    Exchange,
    Transcend,
}