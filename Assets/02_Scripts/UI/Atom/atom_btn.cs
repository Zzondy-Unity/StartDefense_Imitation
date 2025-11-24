using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class atom_btn : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TextMeshProUGUI _text;

    public bool interactable
    {
        get => _button.interactable;
        set => _button.interactable = value;
    }

    public string text
    {
        get => _text.text;
        set => _text.text = value;
    }

    public Color textColor
    {
        get => _text.color;
        set => _text.color = value;
    }

    public Color btnColor
    {
        get
        {
            if (_button.image != null)
            {
                return _button.image.color;
            }
            else
            {
                return Color.black;
            }
        }
        set
        {
            if (_button.image != null)
            {
                _button.image.color = value;
            }
        }
    }

    private void Awake()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }

        if (_text == null)
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
    }

    public void Set(string btnName, Action onClick)
    {
        _button.onClick.RemoveAllListeners();
        
        _text.text = btnName;
        _button.onClick.AddListener(() => onClick?.Invoke());
    }

    public void Set(string btnName, Action onClick, Color btnColor, Color textColor)
    {
        _button.onClick.RemoveAllListeners();
        
        _text.text = btnName;
        _button.onClick.AddListener(() => onClick?.Invoke());
        this.btnColor = btnColor;
        this.textColor =  textColor;
    }
}
