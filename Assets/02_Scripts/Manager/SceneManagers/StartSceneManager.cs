using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : BaseSceneManager
{
    public Button startButton;


    public override SceneName curScene { get; } = SceneName.StartScene;
    public override void Init()
    {
        startButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Scene.LoadScene(SceneName.LobbyScene);
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }
}
