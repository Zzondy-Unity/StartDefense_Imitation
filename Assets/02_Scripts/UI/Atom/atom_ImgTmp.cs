using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_ImgTmp : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    
    public string text 
    {get
    {
        return _text.text;
    }
    set
    {
        _text.text = value;
    }
    }

    public void Set(Image image, TextMeshProUGUI text)
    {
        this._image = image;
        this._text = text;
    }
}
