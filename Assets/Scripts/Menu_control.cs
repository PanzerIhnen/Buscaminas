using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_control : MonoBehaviour
{
    public void OnButtonClick(int level)
    {
        PlayerPrefs.SetInt("GameLevel", level);

        SceneManager.LoadScene("SampleScene");
    }
}
