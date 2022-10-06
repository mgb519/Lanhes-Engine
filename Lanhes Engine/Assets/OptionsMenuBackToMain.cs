using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuBackToMain : MonoBehaviour
{
    public void Return() {

        PlayerPrefs.Save();
        SceneManager.LoadScene("Title");
    }
}
