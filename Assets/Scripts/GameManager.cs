using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager GameMgr;

    [SerializeField]
    private int _width;
    [SerializeField]
    private int _hegiht;
    [SerializeField]
    private float _maxBombProbability;
    [SerializeField]
    private float _minBombProbability;
    [SerializeField]
    private GameObject _mapButtonPrefab;
    [SerializeField]
    private GameObject _mapPanel;
    [SerializeField]
    private GameObject _winPanel;
    [SerializeField]
    private TextMeshProUGUI _movesText;
    [SerializeField]
    private GameObject _emojiButton;
    [SerializeField]
    private Sprite[] _emojis;

    public bool Clicked { get; set; }

    public enum States
    {
        Safe,
        Flag,
        Neutral,
        Win,
        GameOver
    }

    private MapButton_control[,] _mapButtons;
    private int _bombsNumber;
    private int _movesToWin;
    private bool _gameOver;

    private void Start()
    {
        GameMgr = this;

        SetLevel();

        _mapButtons = new MapButton_control[_width, _hegiht];
        _bombsNumber = GetBombsNumber();
        _movesToWin = (_width * _hegiht) - _bombsNumber;
        _movesText.text = _movesToWin.ToString();

        CreateButtons();
    }

    public void FirstClick(int x, int y)
    {
        Clicked = true;
        PlaceBombs(x, y);
    }

    public int BombsArround(int x, int y)
    {
        int bombsCount = 0;

        List<MapButton_control> buttonsArround = GetArround(x, y);

        foreach (MapButton_control mapButton in buttonsArround)
        {
            if (mapButton.HasBomb)
            {
                bombsCount++;
            }
        }

        return bombsCount;
    }

    public void CleanArround(int x, int y)
    {
        List<MapButton_control> buttonsArround = GetArround(x, y);

        foreach (MapButton_control mapButton in buttonsArround)
        {
            if (mapButton.Active)
            {
                mapButton.CleanButton();
            }
        }
    }

    public void Explode()
    {
        _gameOver = true;
        SetState(States.GameOver);

        foreach (MapButton_control mapButton in _mapButtons)
        {
            if (mapButton.Active)
            {
                mapButton.CleanButton();
            }
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetState(States state)
    {
        _emojiButton.GetComponent<Image>().sprite = _emojis[(int)state];
    }

    public void Move()
    {
        if (!_gameOver)
        {
            _movesToWin--;

            _movesText.text = _movesToWin.ToString();

            if (_movesToWin == 0)
            {
                SetState(States.Win);
                _winPanel.SetActive(true);
            }
        }
    }

    public void OnMenuClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void CreateButtons()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _hegiht; y++)
            {
                _mapButtons[x, y] = Instantiate(_mapButtonPrefab, _mapPanel.transform).GetComponent<MapButton_control>();
                _mapButtons[x, y].X = x;
                _mapButtons[x, y].Y = y;
            }
        }
    }

    private int GetBombsNumber()
    {
        int max = Convert.ToInt32(_width * _hegiht * _maxBombProbability);
        int min = Convert.ToInt32(_width * _hegiht * _minBombProbability);

        return UnityEngine.Random.Range(min, max);
    }

    private void PlaceBombs(int mercyX, int mercyY)
    {
        for (int i = 0; i < _bombsNumber; i++)
        {
            int x = UnityEngine.Random.Range(0, _width);
            int y = UnityEngine.Random.Range(0, _hegiht);
            if (_mapButtons[x, y].HasBomb || (x == mercyX && y == mercyY))
            {
                i--;
            }
            else
            {
                _mapButtons[x, y].HasBomb = true;
            }
        }
    }

    private List<MapButton_control> GetArround(int x, int y)
    {
        List<MapButton_control> arround = new List<MapButton_control>();

        for (int i = x - 1; i <= x + 1 ; i++)
        {
            if (i >= 0 && i < _width)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j >= 0 && j < _hegiht)
                    {
                        if (i != x || j != y)
                        {
                            arround.Add(_mapButtons[i, j]);
                        }
                    }
                }
            }
        }

        return arround;
    }

    private void SetLevel()
    {
        int level = PlayerPrefs.GetInt("GameLevel", 2);

        switch (level)
        {
            case 0:
                _width = 5;
                _hegiht = 10;
                break;
            case 1:
                _width = 10;
                _hegiht = 10;
                break;
            case 2:
                _width = 20;
                _hegiht = 10;
                break;
            default:
                break;
        }
    }
}
