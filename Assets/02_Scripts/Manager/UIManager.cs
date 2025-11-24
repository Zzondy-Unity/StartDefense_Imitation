using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// UI의 위치를 정의하는 열거형입니다.
/// </summary>
public enum eUIPosition
{
    UI,         // 일반 UI
    Popup,      // 팝업 UI
    Navigator   // 네비게이터 UI
}

/// <summary>
/// UI 시스템을 관리하는 매니저 클래스입니다.
/// UI의 생성, 표시, 숨김 등을 처리합니다.
/// </summary>
public class UIManager : MonoBehaviour, IManager
{
    // UI들의 부모 Transform 리스트 (UI, Popup, Navigator 각각의 캔버스)
    [SerializeField] private List<Transform> parents;

    // 현재 열려있는 UI 딕셔너리 (타입별로 관리)
    private Dictionary<System.Type, GameObject> openUI = new Dictionary<System.Type, GameObject>();
    // 닫혀있는 UI 딕셔너리 (재사용을 위해 보관)
    private Dictionary<System.Type, GameObject> closeUI = new Dictionary<System.Type, GameObject>();

    // 가장 앞에 표시되는 UI
    public UIBase FrontUI;

    /// <summary>
    /// UI 매니저를 초기화합니다.
    /// </summary>
    public void Init()
    {
        // UI 부모 객체들을 생성하고 설정
        if (parents == null) parents = new List<Transform>();
        if (parents.Count == 0)
        {
            List<Transform> trList = new List<Transform>()
            {
                // 각 UI 타입별 캔버스 생성
                // TODO : 경로수정
                Instantiate(GameManager.Resource.LoadAsset<GameObject>("UI/@UI")).transform,
                Instantiate(GameManager.Resource.LoadAsset<GameObject>("UI/@Popup")).transform,
                Instantiate(GameManager.Resource.LoadAsset<GameObject>("UI/@Navigation")).transform
            };
            SetParents(trList);
            // 씬 전환시에도 유지되도록 설정
            foreach (var tr in trList) DontDestroyOnLoad(tr.gameObject);
        }
    }

    /// <summary>
    /// UI 부모 Transform들을 설정합니다.
    /// </summary>
    public void SetParents(List<Transform> parents)
    {
        this.parents = parents;
    }

    /// <summary>
    /// 지정된 타입의 UI를 표시합니다.
    /// </summary>
    /// <typeparam name="T">표시할 UI의 타입</typeparam>
    /// <param name="param">UI에 전달할 매개변수</param>
    /// <returns>생성된 UI 인스턴스</returns>
    public T Show<T>(params object[] param) where T : UIBase
    {
        System.Type type = typeof(T);

        bool isOpen = false;
        var ui = Get<T>(out isOpen);

        if (ui == null)
        {
            Logger.ErrorLog($"{type} does not exit.");
            return null;
        }

        if (isOpen)
        {
            Logger.Log($"{type} is already open."); 
            return ui;
        }

        // UI를 적절한 부모 아래에 배치하고 초기화
        var siblingIndex = parents[(int)ui.uiPosition].childCount;
        ui.gameObject.transform.SetParent(parents[(int)ui.uiPosition], false);
        ui.transform.SetSiblingIndex(siblingIndex);
        ui.opened?.Invoke(param);
        ui.SetActive(ui.uiOptions.isActiveOnLoad);
        ui.uiOptions.isActiveOnLoad = true;

        FrontUI = ui;
        openUI[type] = ui.gameObject;

        return (T)ui;
    }

    /// <summary>
    /// 지정된 타입의 UI를 숨깁니다.
    /// </summary>
    /// <typeparam name="T">숨길 UI의 타입</typeparam>
    /// <param name="param">UI에 전달할 매개변수</param>
    public void Hide<T>(params object[] param) where T : UIBase
    {
        System.Type type = typeof(T);

        bool isOpen = false;
        var ui = Get<T>(out isOpen);
        eUIPosition up = ui.uiPosition;

        if (isOpen)
        {
            openUI.Remove(type);
            ui.closed.Invoke(param);

            if (ui.uiOptions.isDestroyOnHide)
            {
                Destroy(ui.gameObject);
            }
            else
            {
                ui.SetActive(false);
                closeUI[type] = ui.gameObject;
                ui.transform.SetAsFirstSibling();
            }
        }
        UpdateFrontUI(up);
    }


    /// <summary>
    /// 지정된 타입의 UI를 가져옵니다.
    /// </summary>
    /// <typeparam name="T">가져올 UI의 타입</typeparam>
    /// <param name="isOpened">UI가 열려있는지 여부</param>
    /// <returns>가져온 UI 인스턴스</returns>
    public T Get<T>(out bool isOpened) where T : UIBase
    {
        System.Type type = typeof(T);

        UIBase ui = null;
        isOpened = false;

        if (openUI.ContainsKey(type))
        {
            ui = openUI[type].GetComponent<UIBase>();
            isOpened = true;
        }
        else if (closeUI.ContainsKey(type))
        {
            ui = closeUI[type].GetComponent<UIBase>();
            // closeUI.Remove(type);
        }
        else
        {
            var prefabName = type.Name;
            var prefab = Resources.Load("UI/" + prefabName) as GameObject;
            if (prefab == null)
            {
                Logger.ErrorLog($"UI 프리팹을 찾을 수 없습니다: Resources/UI/{prefabName}");
                return null;
            }
            GameObject go = Instantiate(prefab);
            ui = go.GetComponent<UIBase>();
            if (ui == null)
            {
                Logger.ErrorLog($"생성된 프리팹에서 {type} 컴포넌트를 찾을 수 없습니다.");
                Destroy(go);
                return null;
            }
        }

        return (T)ui;
    }

    private void UpdateFrontUI(eUIPosition up)
    {
        if (parents[(int)up].childCount <= 0) return;
        
        FrontUI = null;
        var lastChild = parents[(int)up].GetChild(parents[(int)up].childCount - 1);
        if (lastChild)
        {
            UIBase baseUI = lastChild.gameObject.GetComponent<UIBase>();
            FrontUI = baseUI.gameObject.activeInHierarchy ? baseUI : null;
        }
    }

    /// <summary>
    /// 지정된 타입의 UI가 열려있는지 확인합니다.
    /// </summary>
    /// <typeparam name="T">확인할 UI의 타입</typeparam>
    /// <returns>열려있는 UI 인스턴스</returns>
    public UIBase IsOpened<T>() where T : UIBase
    {
        var uiType = typeof(T);
        return openUI.ContainsKey(uiType) ? openUI[uiType].GetComponent<UIBase>() : null;
    }

    /// <summary>
    /// 열려있는 UI가 있는지 확인합니다.
    /// </summary>
    /// <returns>열려있는 UI가 있는지 여부</returns>
    public bool ExistOpenUI()
    {
        return FrontUI != null;
    }

    /// <summary>
    /// 현재 가장 앞에 표시되는 UI를 가져옵니다.
    /// </summary>
    /// <returns>현재 가장 앞에 표시되는 UI 인스턴스</returns>
    public UIBase GetCurrentFrontUI()
    {
        UpdateFrontUI(eUIPosition.Popup);
        return FrontUI;
    }

    /// <summary>
    /// 팝업UI를 가장 앞으로 이동시킵니다.
    /// </summary>
    /// <param name="popup">가장 앞으로 가게할 UI</param>
    public void BringToFront(UIPopUp popup)
    {
        if (popup == null || !popup.isActiveAndEnabled) return;

        Transform parent = popup.transform.parent;
        if (parent != null)
        {
            popup.transform.SetSiblingIndex(parent.childCount - 1);
            FrontUI = popup;
        }
    }

    public void RefreshAll()
    {
        foreach (var ui in openUI)
        {
            ui.Value.TryGetComponent(out UIBase uiBase);
            if (uiBase is UIPopUp popup)
            {
                popup.Refresh();
            }
        }
    }

    public void Refresh<T>() where T : UIBase
    {
        var ui = IsOpened<T>();
        if (ui != null)
        {
            ui.Refresh();
        }
    }

    /// <summary>
    /// 인디케이터를 표시합니다.
    /// </summary>
    public static void ShowIndicator()
    {

    }

    /// <summary>
    /// 인디케이터를 숨깁니다.
    /// </summary>
    public static void HideIndicator()
    {

    }
}