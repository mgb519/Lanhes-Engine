using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public int partyID;
    public string firstScene;
    public string firstPawnPoint;

    [SerializeField]
    private AudioMixerGroup group;


    public void Awake() {
        if (!PlayerPrefs.HasKey("language")) {
            PlayerPrefs.SetString("language","he");
        }
        LocalizationSettings.SelectedLocale =LocalizationSettings.AvailableLocales.GetLocale(PlayerPrefs.GetString("language"));


        foreach (string n in new string[] { "VolumeMaster", "VolumeMusic", "VolumeSFX"}) {
            if (!PlayerPrefs.HasKey(n)) {
                PlayerPrefs.SetFloat(n, 0.5f);
            }
            group.audioMixer.SetFloat(n, PlayerPrefs.GetFloat(n));

        }



        PlayerPrefs.Save();


    }
    public void NewGame()
    {
        PartyManager.SpawnPlayer(partyID);
        //FIXME: the way this works, we get a warning about having multiple listeners in the scene. This is becuase the main menu scene has a camera with a listener in it too, and the player coexists with it. This goes away after reaching the next scene, and the only consequence will be main menu sound being screwed up for a few frames.
        GameSceneManager.StartLoadScene(firstScene, firstPawnPoint);

    }


    public void LoadGame()
    {

        WindowManager.CreateLoadWindow(null);
    }


    public void Options() {
        SceneManager.LoadScene("Options");
    }
}
