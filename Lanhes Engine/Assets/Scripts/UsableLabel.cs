using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableLabel : MonoBehaviour
{
    private UseTrigger displayedObject;
    private Camera cam;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] RectTransform rectToPosition;


    Vector3 offset;

    bool followMouse = false;

    public void SetOwner(UseTrigger u) {
        displayedObject = u;
        text.text = displayedObject.GetLabelName();
        cam = Camera.main;
        SetPosition();
    }


    public void Update() {
        if (displayedObject.DisplayLabel()) {
            Destroy(gameObject);
        } else {
            SetPosition();
        }
    }


    public void SetPosition() {
        Vector3 r = (followMouse ? Input.mousePosition : cam.WorldToScreenPoint(displayedObject.transform.position)) + offset;
        rectToPosition.position = r;
    }
}
