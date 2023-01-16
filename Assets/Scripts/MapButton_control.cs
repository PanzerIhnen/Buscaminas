using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapButton_control : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshProUGUI _buttonText;

    [Header("Colors")]
    [SerializeField]
    private Color _bombColor;
    [SerializeField]
    private Color _normalColor;
    [SerializeField]
    private Color _flagColor;
    [SerializeField]
    private Color[] _bombArroundColors;

    public int X { get; set; }
    public int Y { get; set; }
    public bool HasBomb { get; set; }

    private bool _active = true;
    public bool Active
    {
        get { return _active; }
    }


    public void OnButtonClick()
    {
        if (!GameManager.GameMgr.Clicked)
        {
            GameManager.GameMgr.FirstClick(X, Y);
        }

        if (HasBomb)
        {
            GameManager.GameMgr.Explode();
        }
        else
        {
            GameManager.GameMgr.SetState(GameManager.States.Safe);
        }

        CleanButton();
    }

    public void CleanButton()
    {
        GetComponent<Button>().interactable = false;
        _active = false;

        if (!HasBomb)
        {
            GameManager.GameMgr.Move();
            int bombs = GameManager.GameMgr.BombsArround(X, Y);

            if (bombs > 0)
            {
                _buttonText.text = bombs.ToString();
                _buttonText.color = _bombArroundColors[bombs - 1];
            }
            else
            {
                GameManager.GameMgr.CleanArround(X, Y);
            }
        }
        else
        {
            GetComponent<Image>().color = _bombColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Active)
            {
                GameManager.GameMgr.SetState(GameManager.States.Flag);
                GetComponent<Button>().interactable = false;
                _active = false;
                _buttonText.text = "?";
                GetComponent<Image>().color = _flagColor;
            }
            else
            {
                GameManager.GameMgr.SetState(GameManager.States.Neutral);
                GetComponent<Button>().interactable = true;
                _active = true;
                _buttonText.text = string.Empty;
                GetComponent<Image>().color = _normalColor;
            }
        }
    }
}
