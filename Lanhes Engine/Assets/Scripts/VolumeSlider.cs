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
    


    private Slider s;

    public void Awake() {
        s = GetComponentInChildren<Slider>();
        s.onValueChanged.AddListener(delegate { ValueChanged(); });
    }


    private void ValueChanged() {
        group.audioMixer.SetFloat("Volume"+(group.ToString()),(s.value)*(maximumVolume-minimumVolume)+minimumVolume);
    }

}
