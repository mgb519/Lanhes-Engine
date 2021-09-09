using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ExampleLoadingScreen : LoadingScreen
{
    [SerializeField]
    private TextMeshProUGUI loadingPercent;

    public override void UpdateProgress(float progress) {
        string set = Mathf.RoundToInt(progress * 100f).ToString() + "%";
        loadingPercent.text = set;

    }
}
