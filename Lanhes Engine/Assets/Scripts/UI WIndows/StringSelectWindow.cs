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
        b.transform.position = new Vector3(xPadFromLeft, contentWindow.parent.GetComponent<RectTransform>().sizeDelta.y - newButtonPos);
        newButtonPos += b.GetComponent<RectTransform>().sizeDelta.y;
    }
}
