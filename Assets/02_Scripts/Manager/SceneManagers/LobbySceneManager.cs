using UnityEngine.UI;

public class LobbySceneManager : BaseSceneManager
{
    public override SceneName curScene { get; } = SceneName.LobbyScene;
    public Button startButton;
    
    public override void Init()
    {
        startButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Scene.LoadScene(SceneName.GameScene);
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}
