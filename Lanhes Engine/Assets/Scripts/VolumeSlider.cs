using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    
    [SerializeField]
    private float minimumVolume=-80;
    [SerializeField]
    private float maximumVolume =20;
    //[SerializeField]
    //private AudioMixer mixer;

    [SerializeField]
    private AudioMixerGroup group;


    string paramName { get { return "Volume" + (group.ToString()); } }

    private Slider s;

    public void Awake() {
        s = GetComponentInChildren<Slider>();
        s.onValueChanged.AddListener(delegate { ValueChanged(); });
        s.value = PlayerPrefs.GetFloat(paramName);
    }


    private void ValueChanged() {
        float v = (s.value) * (maximumVolume - minimumVolume) + minimumVolume;
        group.audioMixer.SetFloat(paramName,v);
        PlayerPrefs.SetFloat(paramName, s.value);

    }

}
