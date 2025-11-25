using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class atom_ImgTmp : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public Image image => _image;

    public string text
    {
        get { return _text.text; }
        set { _text.text = value; }
    }

    public Color textColor
    {
        get{ return _text.color;}
        set { _text.color = value; }
    }

    public void Set(Image image, string text)
    {
        this._image = image;
        this._text.text = text;
    }

    public void Set(Sprite sprite, string text)
    {
        this._image.sprite = sprite;
        this._text.text = text;
    }
}