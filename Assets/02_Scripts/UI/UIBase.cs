using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UI의 기본 설정을 정의하는 클래스입니다.
/// </summary>
[System.Serializable]
public class UIOptions
{
    // UI가 로드될 때 자동으로 활성화할지 여부
    public bool isActiveOnLoad = true;
    // UI가 숨겨질 때 파괴할지 여부
    public bool isDestroyOnHide = true;
}

/// <summary>
/// 모든 UI의 기본이 되는 베이스 클래스입니다.
/// UI의 기본적인 동작과 상태를 관리합니다.
/// </summary>
public class UIBase : MonoBehaviour
{
    // UI의 위치를 정의하는 열거형 값 (UI, Popup, Navigator 중 하나)
    public eUIPosition uiPosition;
    // UI의 기본 설정 옵션
    public UIOptions uiOptions;
    // UI 애니메이션을 위한 Animator 컴포넌트
    public Animator uiAnim;

    // UI가 열릴 때 호출되는 이벤트
    public UnityAction<object[]> opened;
    // UI가 닫힐 때 호출되는 이벤트
    public UnityAction<object[]> closed;

    /// <summary>
    /// 컴포넌트가 초기화될 때 이벤트 핸들러를 설정합니다.
    /// </summary>
    protected virtual void Awake()
    {
        opened = Opened;
        closed = Closed;
    }

    /// <summary>
    /// UI의 활성화 상태를 설정합니다.
    /// </summary>
    /// <param name="isActive">활성화 여부</param>
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// UI를 직접 숨기는 가상 메서드입니다.
    /// 하위 클래스에서 재정의하여 사용할 수 있습니다.
    /// </summary>
    public virtual void HideDirect() { }

    /// <summary>
    /// UI가 열릴 때 호출되는 가상 메서드입니다.
    /// </summary>
    /// <param name="param">UI 열기에 필요한 매개변수 배열</param>
    public virtual void Opened(object[] param)
    {
        // TODO : 애니메이션사용 여부 확인 필요 Close 포함
        // UI 열기 애니메이션 실행
        if (uiAnim != null) uiAnim.SetTrigger("OpenUI");

        // UI의 위치와 크기를 초기화
        var rectTf = GetComponent<RectTransform>();
        rectTf.localPosition = Vector3.zero;
        rectTf.localScale = Vector3.one;
        rectTf.offsetMin = Vector3.zero;
        rectTf.offsetMax = Vector3.zero;
    }

    /// <summary>
    /// UI가 닫힐 때 호출되는 가상 메서드입니다.
    /// </summary>
    /// <param name="param">UI 닫기에 필요한 매개변수 배열</param>
    public virtual void Closed(object[] param)
    {
        // UI 닫기 애니메이션 실행
        if (uiAnim != null) uiAnim.SetTrigger("CloseUI");
    }
}