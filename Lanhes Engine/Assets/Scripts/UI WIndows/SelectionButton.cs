using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectionButton:MonoBehaviour
{
    private Button button;
    public ISelectable dat;
    public void Awake() {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(PushMessageUp);
    }

    public void PushMessageUp() {
        SelectionWindow parentWindow = GetComponentInParent<SelectionWindow>();
        parentWindow.HandleItem(dat);
    }

}
