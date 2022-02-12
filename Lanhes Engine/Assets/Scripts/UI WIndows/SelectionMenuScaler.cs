using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SelectionMenuScaler : UIBehaviour
{



    public RectTransform ContentToGrowWith;
    private RectTransform t;
    new void Awake() {
        t = GetComponent<RectTransform>();
        base.Awake();
        Debug.Log("anchors:" + (t.anchorMin - t.anchorMax).y);
    }

    //TODO make sure this is only called when menu is updated...
    public void Update() {

        float contentHeight = ContentToGrowWith.sizeDelta.y;
        Debug.Log("content:" + contentHeight);
        if (contentHeight < t.sizeDelta.y) {
            t.sizeDelta = new Vector2(0, contentHeight);
        } else {
            t.sizeDelta = new Vector2(0, t.sizeDelta.y);
        }
        Destroy(this);//TODO I don't quite like the solution. It rather calcifies how selection menus are to be used; can't the Update() fn be replaced with one that just fires whenever children change?
    }
}
