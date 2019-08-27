using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringSelectWindow : ListSelectMenuWindow<string> {

    public float yPadFromTop = 10;
    public float xPadFromLeft = 10;

    private float newButtonPos;


    public void Awake() {
        newButtonPos = yPadFromTop;
    }


    public override void PositionButton(ref ListMenuEntryButton button, Transform contentWindow) {
        Button b = button.GetComponent<Button>();
        RectTransform myRect = b.GetComponent<RectTransform>();
        b.transform.position = new Vector3(xPadFromLeft + myRect.sizeDelta.x, -myRect.sizeDelta.y - newButtonPos);
        newButtonPos += myRect.sizeDelta.y;
    }
}
