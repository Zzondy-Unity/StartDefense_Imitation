using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IManager
{
    private PlayerInputActionMap actionMap;
    public event Action<Vector2> onClick;

    
    private bool isMouseLocked = false;
    public bool IsMouseLocked
    {
        get
        {
            return isMouseLocked;
        }
        set
        {
            isMouseLocked = value;
        }
    }
    

    public void Init()
    {
        actionMap = new PlayerInputActionMap();
        
        actionMap.Enable();
        actionMap.inputMap.mouseClick.performed -= OnClick;
        actionMap.inputMap.mouseClick.performed += OnClick;
        
        IsMouseLocked = true;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!isMouseLocked && context.performed)
        {
            Vector2 mousePos = GetMousePosition();
            onClick?.Invoke(mousePos);
        }
    }


    public Vector2 GetMousePosition()
    {
        if (isMouseLocked) return Vector2.zero;
        Vector2 mousePosition = actionMap.inputMap.mousePosition.ReadValue<Vector2>();
        return mousePosition;
    }
}
